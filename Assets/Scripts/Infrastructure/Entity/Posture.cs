using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchitecturePrototype.Infrastructure.Entity
{
    public struct Posture
    {
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public Posture(Transform t)
        {
            Position = t.position;
            Rotation = t.rotation;
        }
        public Posture(Vector3 p, Quaternion q)
        {
            Position = p;
            Rotation = q;
        }
    }

    public struct SelfTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public SelfTransform(Transform t) : this(t.position, t.rotation, t.lossyScale) { }
        public SelfTransform(Vector3 p, Quaternion r, Vector3 s)
        {
            Position = p;
            Rotation = r;
            Scale = s;
        }
    }
}
