using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    public class Viewfinder : MonoBehaviour, IInputClickHandler
    {
        private Subject<Unit> onClicked = new Subject<Unit>();
        public IObservable<Unit> OnClicked => onClicked.AsObservable();

        public void Enable()
        {
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void OnInputClicked(InputClickedEventData eventData)
        {
            onClicked.OnNext(Unit.Default);
        }

        private void OnEnable()
        {
            InputManager.Instance.AddGlobalListener(gameObject);
        }
        private void OnDisable()
        {
            InputManager.Instance.RemoveGlobalListener(gameObject);
        }
    }
}