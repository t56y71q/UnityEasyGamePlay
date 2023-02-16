using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class ECoroutine
    {
        private UnityTick unityTick;

        internal void Init(GameObject gameObject)
        {
            this.unityTick = gameObject.GetComponent<UnityTick>();
            CoroutineTask.SetAction(StopCoroutine);
        }

        internal void Destroy()
        {
           
        }

        public void StartCoroutine(CoroutineTask coroutineTask)
        {
            unityTick.StartCoroutine(coroutineTask.CallWarpper());
        }

        public void StopCoroutine(CoroutineTask coroutineTask)
        {
            coroutineTask.isStoped = true;
        }
    }
}
