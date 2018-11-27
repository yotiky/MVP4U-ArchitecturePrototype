using ArchitecturePrototype.Model.UseCase;
using ArchitecturePrototype.UI.View;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.Presenter
{
    public abstract class PresenterBase : MonoBehaviour
    {
        protected CompositeDisposable compositeDisposables = new CompositeDisposable();

        public abstract UseCasesBase UseCase { get; }

        protected MessageView MessageView => MessageView.Instance;

        public virtual void InitializeOnAwake()
        {

        }

        public virtual void InitializeOnStart()
        {

        }

        private void OnDestroy()
        {
            compositeDisposables.Clear();
        }
    }
}