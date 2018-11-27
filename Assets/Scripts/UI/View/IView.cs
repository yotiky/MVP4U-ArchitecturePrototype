using ArchitecturePrototype.UI.Component;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public interface IView
    {
        bool IsActive { get; }
        void Initialize();
        void Show();
        void Hide();
        void Enable();
        void Disable();
    }
}