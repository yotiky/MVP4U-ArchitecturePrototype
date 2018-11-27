using ArchitecturePrototype.Infrastructure.Entity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    public class Stage : MonoBehaviour
    {
        public void SetPosition(Posture p)
        {
            transform.position = p.Position;
            transform.rotation = p.Rotation;
        }
    }
}