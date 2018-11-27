using System.Collections;
using System.Collections.Generic;
using ArchitecturePrototype.Model.Engine;
using ArchitecturePrototype.Model.UseCase;
using ArchitecturePrototype.UI.View;
using HoloToolkit.UX.Progress;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Presenter
{
    public class BottleDetectionPresenter : PresenterBase
    {
        private BottleDetectionView view;

        private BottleDetectionUseCases usecase;

        public override UseCasesBase UseCase => usecase;

        public override void InitializeOnStart()
        {
            base.InitializeOnStart();

            view = GetComponent<BottleDetectionView>();
            usecase = new BottleDetectionUseCases(compositeDisposables);

            view.Initialize();
            view.OnTakePhoto
                .Do(_ =>
                {
                    if (usecase.CanTakePhoto.Value)
                    {
                        Debug.Log("take photo...");
                        ProgressIndicator.Instance.Open(IndicatorStyleEnum.AnimatedOrbs, ProgressStyleEnum.None, ProgressMessageStyleEnum.Visible, "Now loading...");
                        usecase.TakePhoto();
                    }
                })
                .Subscribe()
                .AddTo(this);

            view.OnSkipChapter
                .Do(_ =>
                {
                    usecase.SavePosition(Camera.main.transform.position + new Vector3(0, 0, 1f));
                    usecase.TransitScenario();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnBottleDetected
                .Do(p =>
                {
                    ProgressIndicator.Instance.Close();
                    if (p.Positions.Length < 1)
                    {
                        MessageView.Instance.WriteMessage("more tap!!!");
                        return;
                    }

                    Debug.Log("bottle detected...");
                    usecase.SavePosition(p.Positions[0]);
                    usecase.TransitScenario();

                })
                .Subscribe()
                .AddTo(this);

            usecase.OnAfterTransitScenario
                .Do(_ =>
                {
                    MessageView.Instance.WriteMessage("ボトルに向かってタップしてください");
                    view.Enable();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnBeforeTransitScenario
                .Do(c =>
                {
                    view.Disable();
                })
                .Subscribe()
                .AddTo(this);
        }
    }
}