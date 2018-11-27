using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.UI.Component;
using ArchitecturePrototype.UI.FrontCollaborator;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class ManipulationView : MonoBehaviour, IView
    {
        public Stage stage;
        public ManipulationSpot spot;
        public GameObject projectionScreenTarget;
        private ProjectionScreen screen;

        public bool IsActive => gameObject.activeInHierarchy;

        private Subject<Unit> onSkipChapter = new Subject<Unit>();
        public IObservable<Unit> OnSkip => onSkipChapter;

        public void Initialize()
        {
            spot.Initialize();
            screen = new ProjectionScreen(gameObject, spot, projectionScreenTarget);
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

        public void SetPosition(Posture p)
        {
            stage.SetPosition(p);
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