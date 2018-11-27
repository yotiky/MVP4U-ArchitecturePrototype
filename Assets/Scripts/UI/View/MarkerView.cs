using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class MarkerView : MonoBehaviour, IView
    {
        public MarkerTracker tracker;

        public bool IsActive => gameObject.activeInHierarchy;

        private Subject<Posture> onSkipChapter = new Subject<Posture>();
        public IObservable<Posture> OnAnchored { get; private set; }

        public void Initialize()
        {
            OnAnchored = Observable.Amb(onSkipChapter, tracker.OnClicked);
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
                    onSkipChapter.OnNext(new Posture());
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