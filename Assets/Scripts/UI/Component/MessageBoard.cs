using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace ArchitecturePrototype.UI.Component
{
    public class MessageBoard : MonoBehaviour
    {
        public Text text;
        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            canvas.enabled = true;
        }
        public void Hide()
        {
            canvas.enabled = false;
        }

        public void Write(string msg)
        {
            text.text = msg;
        }
        public void Clear()
        {
            text.text = string.Empty;
        }
    }
}