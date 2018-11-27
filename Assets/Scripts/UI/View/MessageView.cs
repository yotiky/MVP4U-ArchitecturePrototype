using ArchitecturePrototype.UI.Component;
using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.View
{
    public class MessageView : Singleton<MessageView>, IView
    {
        public MessageBoard board;

        public bool IsActive => gameObject.activeInHierarchy;

        private void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            ClearMessage();
            Enable();
        }

        public void Enable()
        {
            board.Enable();
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            board.Disable();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            board.Show();
        }

        public void Hide()
        {
            board.Hide();
        }

        public void WriteMessage(string msg)
            => board.Write(msg);
        public void ClearMessage()
            => board.Clear();
    }
}