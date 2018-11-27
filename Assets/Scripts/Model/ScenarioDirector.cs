using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.Model.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using Vuforia;

namespace ArchitecturePrototype.Model.Engine
{
    public class ScenarioDirector
    {
        private readonly List<UseCasesBase> useCases = new List<UseCasesBase>();
        private Scenario scenario = new Scenario();
        private ScenarioContext context = new ScenarioContext();

        public Scenario.Chapter CurrentChapter => scenario.Current.Value;
        public ScenarioContext Context => context;

        public void AddUseCase(UseCasesBase u) => useCases.Add(u);

        public void Transit(Scenario.Chapter completed)
        {
            foreach (var u in useCases)
            {
                if (u.BeforeTransitTrigger.HasFlag(completed))
                {
                    u.BeforeTransitScenario(completed);
                }
            }

            Debug.Log($"## Transit from {completed}");
            scenario.Transit(completed);
            Debug.Log($"## Transit to {CurrentChapter}");

            foreach (var u in useCases)
            {
                if (u.AfterTransitTrigger.HasFlag(CurrentChapter))
                {
                    u.AfterTransitScenario(CurrentChapter);
                }
            }
        }
    }
}
