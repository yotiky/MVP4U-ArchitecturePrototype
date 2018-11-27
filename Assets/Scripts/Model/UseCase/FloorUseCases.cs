using System;
using System.Collections;
using System.Collections.Generic;
using ArchitecturePrototype.Model.Engine;
using HoloToolkit.Unity.SpatialMapping;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Model.UseCase
{
    public class FloorUseCases : UseCasesBase
    {
        protected override Scenario.Chapter PlayChapter => Scenario.Chapter.ScanFloor;
        public override Scenario.Chapter BeforeTransitTrigger => PlayChapter | Scenario.Chapter.Start;

        public FloorUseCases(CompositeDisposable disposables) 
            : base(disposables)
        {
            OnAfterTransitScenario
                .Do(_ =>
                {
                    SpatialMappingManager.Instance.gameObject.SetActive(true);
                })
                .Subscribe()
                .AddTo(disposables);

            OnBeforeTransitScenario
                .Do(c =>
                {
                    if (c == Scenario.Chapter.Start)
                    {
                        SpatialMappingManager.Instance.gameObject.SetActive(true);
                    }
                    else
                    {
                        SpatialMappingManager.Instance.gameObject.SetActive(false);
                    }
                })
                .Subscribe()
                .AddTo(disposables);
        }
        
        public void SetFloorElevation(float y)
        {
            director.Context.FloorElevation = y;
        }
    }
}