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
    public class Scenario
    {
        [Flags]
        public enum Chapter
        {
            None = 0,
            Start = 1,
            HeadsetAdjustment = 1 << 1,
            MarkerTracking = 1 << 2,
            ScanFloor = 1 << 3,
            BottleDetection = 1 << 4,
            Manipulation = 1 << 5,
            End = 1 << 6,
        }

        private ReactiveProperty<Chapter> current = new ReactiveProperty<Chapter>();
        public ReadOnlyReactiveProperty<Chapter> Current => current.ToReadOnlyReactiveProperty();

        public void Transit(Chapter completed)
        {
            switch(completed)
            {
                case Chapter.Start:
                    current.Value = Chapter.HeadsetAdjustment;
                    break;
                case Chapter.HeadsetAdjustment:
                    current.Value = Chapter.MarkerTracking;
                    break;
                case Chapter.MarkerTracking:
                    current.Value = Chapter.ScanFloor;
                    break;
                case Chapter.ScanFloor:
                    current.Value = Chapter.BottleDetection;
                    break;
                case Chapter.BottleDetection:
                    current.Value = Chapter.Manipulation;
                    break;
                case Chapter.Manipulation:
                    current.Value = Chapter.HeadsetAdjustment;
                    //current.Value = Chapter.End;
                    break;
                case Chapter.End:
                    break;
            }
        }
    }

    public class ScenarioContext
    {
        public Posture ReferencePoint { get; set; }
        public float FloorElevation { get; set; }
        public Vector3 HandManipulationTarget { get; set; }
    }
}
