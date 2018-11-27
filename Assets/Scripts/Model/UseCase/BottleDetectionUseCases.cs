using System;
using System.Collections;
using System.Collections.Generic;
using ArchitecturePrototype.Model.Device;
using ArchitecturePrototype.Model.Engine;
using ArchitecturePrototype.Model.ServiceAgent.Azure;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Model.UseCase
{
    public class BottleDetectionUseCases : UseCasesBase
    {
        private PhotoCamera photoCamera;
        private CustomVisionClient client;

        protected override Scenario.Chapter PlayChapter => Scenario.Chapter.BottleDetection;

        public IReadOnlyReactiveProperty<bool> CanTakePhoto;

        private Subject<DetectedStatus> onBottleDetected = new Subject<DetectedStatus>();
        public IObservable<DetectedStatus> OnBottleDetected => onBottleDetected;

        public BottleDetectionUseCases(CompositeDisposable disposables) 
            : base(disposables)
        {
            photoCamera = new PhotoCamera();
            client = new CustomVisionClient();

            CanTakePhoto = Observable.CombineLatest(photoCamera.CanTakePhoto, client.CanPost)
                .Select(x => !x.Contains(false))
                .ToReactiveProperty();

            PhotoCamera.ShootingPlan plan = null;
            byte[] imageData = null;
            photoCamera.OnPhotoCaptured
                .Do(b =>
                {
                    plan = photoCamera.Plan;
                    imageData = b;
                })
                .SelectMany(x => client.Post(x))
                .Do(res =>
                {
                    var positions = client.Convert(plan, imageData, res);
                    var status = new DetectedStatus
                    {
                        Plan = plan,
                        Positions = positions,
                    };
                    onBottleDetected.OnNext(status);
                })
                .Subscribe()
                .AddTo(disposables);
        }
        
        public bool TakePhoto()
        {
            if (!photoCamera.CanTakePhoto.Value)
            {
                Debug.Log("PhotoCameraを使用できませんでした");
                return false;
            }

            photoCamera.TakePhotoAsync();
            return true;
        }

        public void SavePosition(Vector3 p)
        {
            director.Context.HandManipulationTarget = p;
        }

        public class DetectedStatus
        {
            public Vector3[] Positions { get; set; }
            public PhotoCamera.ShootingPlan Plan { get; set; }
        }
    }
}