using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class UnityFixedUpdate:MonoBehaviour
    {
        private List<IFixedUpdate> fixedUpdates = new List<IFixedUpdate>(32);
        private Queue<IFixedUpdate> adds = new Queue<IFixedUpdate>(16);
        private Queue<IFixedUpdate> removes = new Queue<IFixedUpdate>(16);

        private void Awake()
        {
            enabled = false;
        }

        public void AddFixedUpdate(IFixedUpdate fixedUpdate)
        {
            adds.Enqueue(fixedUpdate);
            if(!enabled)
                enabled = true;
        }

        public void RemoveFixedUpdate(IFixedUpdate fixedUpdate)
        {
            removes.Enqueue(fixedUpdate);
        }

        private void FixedUpdate()
        {
            while (adds.Count > 0)
            {
                fixedUpdates.Add(adds.Dequeue());
            }

            while (removes.Count > 0)
            {
                fixedUpdates.Remove(removes.Dequeue());
            }

            if(fixedUpdates.Count==0)
            {
                enabled = false;
            }

            for (int i = 0; i < fixedUpdates.Count; i++)
            {
                fixedUpdates[i].FixedUpdate();
            }
        }
    }
}
