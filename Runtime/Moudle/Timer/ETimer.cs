using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    public class ETimer:ITick
    {
        private List<Timer> updates;
        private Queue<Timer> adds;
        private Queue<Timer> removes;

        private float pointSecond;
        private bool isTicking;

        private const float spanTime = 0.1f;
        private const int count = 20;

        private EPool pool;

        public ETimer(EPool pool)
        {
            this.pool = pool;
        }

        internal void Init()
        {
            updates = new List<Timer>(count);
            adds = new Queue<Timer>(count/2);
            removes = new Queue<Timer>(count / 2);

            Timer.remove = updates.RemoveAt;
            Timer.release = pool.Release<Timer>;

            pool.BuildPool<Timer>(delegate () { return new Timer(); }, delegate (Timer timer) { }, count);
        }

        public Timer StartTimer(float time, Action finish, Action<float> update = null,bool isLoop = false )
        {
            Timer timer = pool.Get<Timer>();
            timer.SetProperty(time, finish, update, isLoop);
            adds.Enqueue(timer);
            if (!isTicking)
            {
                isTicking = true;
                FrameWork.frameWork.AddTick(this);
            }
            return timer;
        }

        public void StopTimer(Timer timer)
        {
            removes.Enqueue(timer);
        }

        internal  void Destroy()
        {
            if (isTicking)
                FrameWork.frameWork.RemoveTick(this);
            pool.DestroyPool<Timer>();
        }

        public void Tick()
        {
            if (removes.Count == 0 && adds.Count == 0 && updates.Count == 0)
            {
                FrameWork.frameWork.RemoveTick(this);
                isTicking = false;
            }

            while (adds.Count > 0)
            {
                updates.Add(adds.Dequeue());
            }

            while (removes.Count > 0)
            {
                updates.Remove(removes.Dequeue());
            }

            if (updates.Count > 0)
            {
                if (pointSecond > spanTime)
                {
                    pointSecond = 0.0f;
                    for (int i = updates.Count - 1; i >= 0; i--)
                    {
                        updates[i].Update(spanTime,i);
                    }
                }
                pointSecond += UnityEngine.Time.deltaTime;
            }
        }
    }
}
