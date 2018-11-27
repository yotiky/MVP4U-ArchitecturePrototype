using ArchitecturePrototype.Infrastructure.Entity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    public class MarkerTracker : MonoBehaviour, IInputClickHandler
    {
        private Subject<Posture> onClicked = new Subject<Posture>();
        public IObservable<Posture> OnClicked => onClicked.AsObservable();

        public void OnInputClicked(InputClickedEventData eventData)
        {
            onClicked.OnNext(new Posture(transform));
        }
    }
}