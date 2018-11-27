using ArchitecturePrototype.Infrastructure.Entity;
using ArchitecturePrototype.UI.Component;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.FrontCollaborator
{
    public class MovableModelBounding : MonoBehaviour
    {
        public InputClickHandler done;
        public IObservable<Unit> Done => done.OnClicked;

        public IReadOnlyReactiveProperty<Vector3> Position { get; private set; }


        void Start()
        {
            Position = transform.ObserveEveryValueChanged(x => x.position).ToReadOnlyReactiveProperty();
            gameObject.SetActive(false);
        }
        
        public void SetTarget(GameObject target)
        {
            var bounds = GetFullBounds(target);
            transform.position = bounds.center;
            transform.rotation.Set(0, 0, 0, 1);
            transform.localScale = bounds.size;

            gameObject.SetActive(true);
        }
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private Bounds GetFullBounds(GameObject target)
        {
            const float scalePadding = 0.05f;

            var boundsPoints = GetRenderBoundsPoints(target);
            if (boundsPoints.Count > 0)
            {
                for (int i = 0; i < boundsPoints.Count; i++)
                {
                    boundsPoints[i] = target.transform.InverseTransformPoint(boundsPoints[i]);
                }

                var localTargetBounds = new Bounds();
                localTargetBounds.center = boundsPoints[0];
                localTargetBounds.size = Vector3.zero;
                foreach (var point in boundsPoints)
                {
                    localTargetBounds.Encapsulate(point);
                }

                var scale = localTargetBounds.size;
                scale.Scale(target.transform.lossyScale);

                float largestDimension = Mathf.Max(Mathf.Max(
                    Mathf.Abs(scale.x),
                    Mathf.Abs(scale.y)),
                    Mathf.Abs(scale.z));


                scale.x += (largestDimension * scalePadding);
                scale.y += (largestDimension * scalePadding);
                scale.z += (largestDimension * scalePadding);

                var targetBounds = new Bounds();
                targetBounds.center = target.transform.TransformPoint(localTargetBounds.center);
                targetBounds.size = scale;
                return targetBounds;
            }

            return new Bounds
            {
                center = target.transform.position,
                size = target.transform.lossyScale,
            };
        }
        private static List<Vector3> GetRenderBoundsPoints(GameObject target)
        {
            var boundsPoints = new List<Vector3>();
            var renderers = target.GetComponentsInChildren<Renderer>();
            foreach (var render in renderers)
            {
                Vector3[] corners = null;
                render.bounds.GetCornerPositionsFromRendererBounds(ref corners);
                boundsPoints.AddRange(corners);
            }
            return boundsPoints;
        }
    }
}