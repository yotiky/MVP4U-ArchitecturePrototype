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
    public class HeadsetAdjustmentUseCases : UseCasesBase
    {
        protected override Scenario.Chapter PlayChapter => Scenario.Chapter.HeadsetAdjustment;

        public HeadsetAdjustmentUseCases(CompositeDisposable disposables) : base(disposables) { }

    }
}
