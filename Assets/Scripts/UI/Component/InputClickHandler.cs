using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    public class InputClickHandler : MonoBehaviour, IInputClickHandler
    {
        private Subject<Unit> onClicked = new Subject<Unit>();
        public IObservable<Unit> OnClicked => onClicked;

        public GameObject SelectedObject { get; private set; } 

        public void OnInputClicked(InputClickedEventData eventData)
        {
            SelectedObject = eventData.selectedObject;
            onClicked.OnNext(Unit.Default);
        }
    }
}