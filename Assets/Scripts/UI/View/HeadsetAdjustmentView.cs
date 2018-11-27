using ArchitecturePrototype.UI.Component;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class HeadsetAdjustmentView : MonoBehaviour, IView
    {
        public HeadsetAdjustment headsetAdjustment;

        public bool IsActive => gameObject.activeInHierarchy;

        private Subject<Unit> onSkipChapter = new Subject<Unit>();
        public IObservable<Unit> OnAdjusted { get; private set; }

        public void Initialize()
        {
            OnAdjusted = Observable.Amb(onSkipChapter, headsetAdjustment.OnClicked);
            Disable();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            headsetAdjustment.Enable();
        }
        public void Disable()
        {
            gameObject.SetActive(false);
            headsetAdjustment.Disable();
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void Hide()
        {
            throw new NotImplementedException();
        }


        private CompositeDisposable enabledTermDisposable;
        private void OnEnable()
        {
            enabledTermDisposable = new CompositeDisposable();
            SkipChapterHandler.Instance.OnKeyUp
                .Do(_ =>
                {
                    onSkipChapter.OnNext(Unit.Default);
                })
                .Subscribe()
                .AddTo(enabledTermDisposable);
        }
        private void OnDisable()
        {
            if (enabledTermDisposable != null)
            {
                enabledTermDisposable.Dispose();
                enabledTermDisposable = null;
            }
        }
    }
}