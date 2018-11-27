using ArchitecturePrototype.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class BottleDetectionView : MonoBehaviour, IView
    {
        public Viewfinder viewfinder;

        public bool IsActive => gameObject.activeInHierarchy;

        private Subject<Unit> onSkipChapter = new Subject<Unit>();
        public IObservable<Unit> OnSkipChapter => onSkipChapter;
        public IObservable<Unit> OnTakePhoto => viewfinder.OnClicked;

        public void Initialize()
        {
            Disable();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
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