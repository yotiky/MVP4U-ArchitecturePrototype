using ArchitecturePrototype.UI.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using ArchitecturePrototype.Model.UseCase;

namespace ArchitecturePrototype.Presenter
{
    public class ManipulationPresenter : PresenterBase
    {
        private ManipulationView view;

        private ManipulationUseCases usecase;

        public override UseCasesBase UseCase => usecase;

        public override void InitializeOnStart()
        {
            base.InitializeOnStart();

            view = GetComponent<ManipulationView>();
            usecase = new ManipulationUseCases(compositeDisposables);

            view.Initialize();
            view.OnSkip
                .Do(_ => usecase.TransitScenario())
                .Subscribe()
                .AddTo(this);

            usecase.OnAfterTransitScenario
                .Do(_ =>
                {
                    MessageView.Instance.WriteMessage("モデルを操作できます");
                    view.SetPosition(usecase.GetPosture());
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