using ArchitecturePrototype.Infrastructure.Entity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace ArchitecturePrototype.UI.FrontCollaborator
{
    public class RelativePositionContainer : MonoBehaviour
    {
        void Start()
        {
            gameObject.SetActive(false);
        }

        private Transform _srcParentTransform;
        private GameObject _includeTarget;

        public void SetTarget(GameObject includeTarget, Vector3 center)
        {
            _srcParentTransform = includeTarget.transform.parent;
            _includeTarget = includeTarget;

            transform.position = center;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            includeTarget.transform.parent = transform;

            gameObject.SetActive(true);
        }
        public void UpdateTo(Vector3 p)
        {
            transform.position = p;
        }
        public void Done()
        {
            _includeTarget.transform.parent = _srcParentTransform;
            _srcParentTransform = null;
            _includeTarget = null;
            gameObject.SetActive(false);
        }
    }
}