using ArchitecturePrototype.Model.UseCase;
using ArchitecturePrototype.UI.View;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Presenter
{
    public class HeadsetAdjustmentPresenter : PresenterBase
    {
        private HeadsetAdjustmentView view;

        private HeadsetAdjustmentUseCases usecase;

        public override UseCasesBase UseCase => usecase;

        public override void InitializeOnStart()
        {
            base.InitializeOnStart();

            view = GetComponent<HeadsetAdjustmentView>();
            usecase = new HeadsetAdjustmentUseCases(compositeDisposables);

            view.Initialize();
            view.OnAdjusted
                .Where(_ => usecase.CurrentChapter == Model.Engine.Scenario.Chapter.HeadsetAdjustment)
                .Do(_ => usecase.TransitScenario())
                .Subscribe()
                .AddTo(this);

            usecase.OnAfterTransitScenario
                .Do(_ =>
                {
                    MessageView.Instance.ClearMessage();
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