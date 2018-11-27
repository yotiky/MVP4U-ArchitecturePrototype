using System.Collections;
using System.Collections.Generic;
using ArchitecturePrototype.Model.Engine;
using ArchitecturePrototype.Model.UseCase;
using ArchitecturePrototype.UI.View;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Presenter
{
    public class FloorPresenter : PresenterBase
    {
        private FloorView view;

        private FloorUseCases usecase;

        public override UseCasesBase UseCase => usecase;

        public override void InitializeOnStart()
        {
            base.InitializeOnStart();

            view = GetComponent<FloorView>();
            usecase = new FloorUseCases(compositeDisposables);

            view.Initialize();
            view.OnFloorScanned
                .Do(f =>
                {
                    usecase.SetFloorElevation(f);
                    usecase.TransitScenario();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnAfterTransitScenario
                .Do(_ =>
                {
                    MessageView.Instance.WriteMessage("卓上に向かってタップしてください");
                    view.Enable();
                })
                .Subscribe()
                .AddTo(this);

            usecase.OnBeforeTransitScenario
                .Do(_ =>
                {
                    view.Disable();
                })
                .Subscribe()
                .AddTo(this);
        }
    }
}