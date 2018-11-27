using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.UI.Component;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.FrontCollaborator
{
    public class ManipulationSpot : MonoBehaviour
    {
        public MovableModelBounding boundingBox;
        public RelativePositionContainer containerTemplate;
        private RelativePositionContainer container;
        public ManipulationTarget target;

        public void Initialize()
        {
            container = Instantiate<RelativePositionContainer>(containerTemplate);

            target.OnClicked
                .Do(_ =>
                {
                    boundingBox.SetTarget(target.gameObject);
                    container.SetTarget(target.gameObject, boundingBox.transform.position);
                })
                .Subscribe()
                .AddTo(this);

            boundingBox.Done
                .Do(_ =>
                {
                    container.Done();
                    boundingBox.Disable();
                })
                .Subscribe()
                .AddTo(this);

            boundingBox.Position
                .Do(p =>
                {
                    container.UpdateTo(p);
                })
                .Subscribe()
                .AddTo(this);
        }
    }
    public class ProjectionScreen
    {
        private ManipulationSpot spot;
        private GameObject screenTarget;
        private RelativePositionContainer container;

        public ProjectionScreen(GameObject parent, ManipulationSpot s, GameObject t)
        {
            spot = s;
            screenTarget = t;
            container = GameObject.Instantiate<RelativePositionContainer>(s.containerTemplate);

            spot.target.OnClicked
                .Do(_ =>
                {
                    container.SetTarget(screenTarget, spot.boundingBox.transform.position);
                })
                .Subscribe()
                .AddTo(parent);

            spot.boundingBox.Done
                .Do(_ =>
                {
                    container.Done();
                })
                .Subscribe()
                .AddTo(parent);

            spot.boundingBox.Position
                .Do(p =>
                {
                    container.UpdateTo(p);
                })
                .Subscribe()
                .AddTo(parent);
        }
    }
}