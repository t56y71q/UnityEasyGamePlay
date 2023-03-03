using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    [Serializable]
    public struct ETransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public ETransform(Transform transform)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }

        public ETransform(Vector3 position,Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = Quaternion.Euler(rotation);
            this.scale = scale;
        }

        public void SetUnityTransform(Transform transform)
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = scale;
        }

        public static ETransform GetOrigin()
        {
            return new ETransform(Vector3.zero, Vector3.zero, Vector3.one);
        }
    }
}
