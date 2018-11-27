using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.Component
{
    [RequireComponent(typeof(KeyboardManager))]
    public class SkipChapterHandler : Singleton<SkipChapterHandler>
    {
        KeyboardManager.KeyboardRegistration keyboardRegistration;

        private Subject<KeyCode> onKeyUp = new Subject<KeyCode>();
        public IObservable<KeyCode> OnKeyUp => onKeyUp.AsObservable();

        private void OnKeyUpCallbak(KeyboardManager.KeyCodeEventPair pair)
        {
            onKeyUp.OnNext(pair.KeyCode);
        }

        private void OnEnable()
        {
            keyboardRegistration = KeyboardManager.Instance.RegisterKeyEvent(
                new KeyboardManager.KeyCodeEventPair(KeyCode.Tab, KeyboardManager.KeyEvent.KeyUp), 
                OnKeyUpCallbak);
        }

        private void OnDisable()
        {
            if (keyboardRegistration != null)
            {
                keyboardRegistration.Dispose();
                keyboardRegistration = null;
            }
        }
    }
}