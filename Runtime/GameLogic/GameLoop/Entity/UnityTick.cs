using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    class UnityTick:MonoBehaviour
    {
        private List<ITick> ticks = new List<ITick>(32);
        private Queue<ITick> adds = new Queue<ITick>(16);
        private Queue<ITick> removes = new Queue<ITick>(16);
        private Queue<Action> nextFrames = new Queue<Action>(16);

        private bool running=false;

        public void AddTick(ITick tick)
        {
            adds.Enqueue(tick);
        }

        public void RemoveTick(ITick tick)
        {
            removes.Enqueue(tick);
        }

        public void NextFrame(Action action)
        {
            nextFrames.Enqueue(action);
        }

        private void Update()
        {
            int length = nextFrames.Count;

            while (adds.Count > 0)
            {
                ticks.Add(adds.Dequeue());
            }

            while (removes.Count > 0)
            {
                ticks.Remove(removes.Dequeue());
            }

            for (int i = 0; i < ticks.Count; i++)
            {
                ticks[i].Tick();
            }

            while (running && --length > 0)
            {
                nextFrames.Dequeue().Invoke();
            }
            running = true;
        }
    }
}
