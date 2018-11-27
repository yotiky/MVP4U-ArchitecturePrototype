using ArchitecturePrototype.UI.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ArchitecturePrototype.Model.UseCase;

namespace ArchitecturePrototype.Presenter
{
    public class MarkerPresenter : PresenterBase
    {
        private MarkerView view;

        private MarkerUseCases usecase;

        public override UseCasesBase UseCase => usecase;

        public override void InitializeOnStart()
        {
            base.InitializeOnStart();

            view = GetComponent<MarkerView>();
            usecase = new MarkerUseCases(compositeDisposables);

            view.Initialize();
            view.OnAnchored
                .Where(_ => usecase.CurrentChapter == Model.Engine.Scenario.Chapter.MarkerTracking)
                .Do(p =>
                {
                    usecase.SetReferencePoint(p);
                    usecase.TransitScenario();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnAfterTransitScenario
                .Do(_ =>
                {
                    MessageView.Instance.WriteMessage("マーカーをタップしてください");
                    view.Enable();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnBeforeTransitScenario
                .Do(_ => view.Disable())
                .Subscribe()
                .AddTo(this);
        }
    }
}