using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.Model.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Model.UseCase
{
    public class ManipulationUseCases : UseCasesBase
    {
        protected override Scenario.Chapter PlayChapter => Scenario.Chapter.Manipulation;

        public ManipulationUseCases(CompositeDisposable disposables) : base(disposables) { }


        public Posture GetPosture()
        {
            // 適当な相対位置をでっち上げる
            var to = director.Context.HandManipulationTarget;
            var from = Camera.main.transform.position;
            var p = (to - from).normalized * 3f;
            var r = director.Context.ReferencePoint.Rotation;
            var y = director.Context.FloorElevation;

            return new Posture(new Vector3(p.x, y, p.z), r);
        }
    }
}
