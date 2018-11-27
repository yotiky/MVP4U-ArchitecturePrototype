using ArchitecturePrototype.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class FloorView : MonoBehaviour, IView
    {
        public FloorScanner scanner;

        public bool IsActive => gameObject.activeInHierarchy;

        private Subject<float> onSkipChapter = new Subject<float>();
        public IObservable<float> OnFloorScanned { get; private set; }

        public void Initialize()
        {
            OnFloorScanned = Observable.Amb(onSkipChapter, scanner.OnClicked);
            Disable();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            scanner.Enable();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            scanner.Disable();
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
                    onSkipChapter.OnNext(-1);
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