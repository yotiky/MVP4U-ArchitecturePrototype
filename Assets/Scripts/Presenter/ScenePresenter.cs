using ArchitecturePrototype.Model.Engine;
using ArchitecturePrototype.UI.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArchitecturePrototype.Presenter
{
    public class ScenePresenter : MonoBehaviour
    {
        public PresenterBase[] presenters;

        void Awake()
        {
            foreach (var p in presenters)
            {
                p.InitializeOnAwake();
            }
        }

        void Start()
        {
            var director = new ScenarioDirector();

            foreach (var p in presenters)
            {
                p.InitializeOnStart();
                p.UseCase.SetDirector(director);
            }

            director.Transit(Scenario.Chapter.Start);
        }
    }
}