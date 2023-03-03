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
        }

        internal void Destroy()
        {
           
        }

        public CoroutineTask StartCoroutine(IEnumerator enumerator)
        {
            CoroutineTask coroutineTask = new CoroutineTask(enumerator);
            unityTick.StartCoroutine(coroutineTask.CallWarpper());
            return coroutineTask;
        }

        public void StopCoroutine(CoroutineTask coroutineTask)
        {
            coroutineTask.isStoped = true;
        }
    }
}
