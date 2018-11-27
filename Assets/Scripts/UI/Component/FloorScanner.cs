using ArchitecturePrototype.Infrastructure.Entity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    public class FloorScanner : MonoBehaviour, IInputClickHandler
    {
        public GameObject gazeCursor;

        private Subject<float> onClicked = new Subject<float>();
        public IObservable<float> OnClicked => onClicked.AsObservable();

        public void Enable()
        {
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private void GazeTracking(long _)
        {
            if (SpatialMappingManager.Instance == null)
            {
                gazeCursor.SetActive(false);
                return;
            }

            var hitObj = GazeManager.Instance.HitObject;
            if (hitObj == null || hitObj.layer != SpatialMappingManager.Instance.PhysicsLayer)
            {
                gazeCursor.SetActive(false);
                return;
            }

            gazeCursor.transform.position = GazeManager.Instance.HitPosition;
            gazeCursor.SetActive(true);
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            var hitObj = GazeManager.Instance.HitObject;
            if (hitObj == null || hitObj.layer != SpatialMappingManager.Instance.PhysicsLayer) { return; }

            var hitPos = GazeManager.Instance.HitPosition;
            onClicked.OnNext(hitPos.y);
        }


        private CompositeDisposable enabledTermDisposable;
        private void OnEnable()
        {
            enabledTermDisposable = new CompositeDisposable();
            InputManager.Instance.AddGlobalListener(gameObject);
            Observable.EveryUpdate()
                .Do(GazeTracking)
                .Subscribe()
                .AddTo(enabledTermDisposable);
        }
        private void OnDisable()
        {
            InputManager.Instance.RemoveGlobalListener(gameObject);
            if (enabledTermDisposable != null)
            {
                enabledTermDisposable.Dispose();
                enabledTermDisposable = null;
            }
        }
    }
}