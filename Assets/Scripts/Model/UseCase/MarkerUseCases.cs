using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.Model.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Vuforia;

namespace ArchitecturePrototype.Model.UseCase
{
    public class MarkerUseCases : UseCasesBase
    {
        protected override Scenario.Chapter PlayChapter => Scenario.Chapter.MarkerTracking;

        public MarkerUseCases(CompositeDisposable disposables) 
            : base(disposables)
        {
            OnAfterTransitScenario
                .Do(_ => VuforiaBehaviour.Instance.enabled = true)
                .Subscribe()
                .AddTo(disposables);

            OnBeforeTransitScenario
                .Do(_ => VuforiaBehaviour.Instance.enabled = false)
                .Subscribe()
                .AddTo(disposables);
        }


        public void SetReferencePoint(Posture p)
        {
            director.Context.ReferencePoint = p;
        }

    }
}
