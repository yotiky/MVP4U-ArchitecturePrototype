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
    public abstract class UseCasesBase
    {
        protected ScenarioDirector director;
        public Scenario.Chapter CurrentChapter => director.CurrentChapter;

        protected abstract Scenario.Chapter PlayChapter { get; }
        public virtual Scenario.Chapter AfterTransitTrigger => PlayChapter;
        public virtual Scenario.Chapter BeforeTransitTrigger => PlayChapter;

        private Subject<Scenario.Chapter> onAfterTransitScenario = new Subject<Scenario.Chapter>();
        public IObservable<Scenario.Chapter> OnAfterTransitScenario => onAfterTransitScenario.AsObservable();

        private Subject<Scenario.Chapter> onBeforeTransitScenario = new Subject<Scenario.Chapter>();
        public IObservable<Scenario.Chapter> OnBeforeTransitScenario => onBeforeTransitScenario.AsObservable();

        protected UseCasesBase(CompositeDisposable disposables) { }


        public void SetDirector(ScenarioDirector d)
        {
            director = d;
            director.AddUseCase(this);
        }

        public virtual void TransitScenario() { TransitScenario(PlayChapter); }
        protected void TransitScenario(Scenario.Chapter from) { director.Transit(from); }
        public virtual void BeforeTransitScenario(Scenario.Chapter c) { onBeforeTransitScenario.OnNext(c); }
        public virtual void AfterTransitScenario(Scenario.Chapter c) { onAfterTransitScenario.OnNext(c); }
    }
}
