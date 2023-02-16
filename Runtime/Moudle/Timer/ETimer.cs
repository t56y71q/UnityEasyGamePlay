using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    public class ETimer:ITick
    {
        private OrderList<TimeStep> updates;
        private Queue<Timer> adds;
        private Queue<Timer> removes;

        private float pointSecond;
        private bool isTicking;

        private const float spanTime = 0.1f;
        private const int count = 20;

        internal void Init()
        {
            updates = new OrderList<TimeStep>(count);
            adds = new Queue<Timer>(count/2);
            removes = new Queue<Timer>(count / 2);

            TimeStep.SetAction(adds.Enqueue, FrameWork.frameWork.ReleasePoolAble<Timer>, updates.Remove);

            FrameWork.frameWork.BuildPool<Timer>(delegate () { return new Timer(); }, delegate (Timer timer) { }, count);
        }

        public Timer StartTimer(float time, Action finish, Action<float> update = null,bool isLoop = false )
        {

            Timer timer = FrameWork.frameWork.GetPoolAble<Timer>();
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
            FrameWork.frameWork.DestroyPool<Timer>();
        }

        public void Tick()
        {
            if (removes.Count == 0 && adds.Count == 0 && updates.Count == 0)
            {
                FrameWork.frameWork.RemoveTick(this);
                isTicking = false;
            }

            TimeStep timeStep;
            Timer timer;
            int i;

            while (adds.Count > 0)
            {
                timer = adds.Dequeue();
                timeStep = new TimeStep(timer);
                i = updates.Find(timeStep);

                if ((uint)i < updates.Count)
                    updates[i].AddTimer(timer);
                else
                    updates.Insert(timeStep);
            }

            while (removes.Count > 0)
            {
                //Timer 的地址作为句柄从updates中查找
                timer = removes.Dequeue();
                for (i = 0; i < updates.Count && !updates[i].RemoveTimer(timer); i++) ;
            }

            if (updates.Count > 0)
            {
                if (pointSecond >= spanTime)
                {
                    pointSecond = 0.0f;
                    for (i = updates.Count - 1; i >= 0; i--)
                    {
                        timeStep = updates[i];
                        timeStep.Update(spanTime);

                        if (timeStep.time <= 0f)
                        {
                            timeStep.Finish(adds);
                            updates.RemoveAt(i);
                        }
                        else
                        {
                            updates.Update(i, timeStep);
                            i--;
                            break;
                        }
                    }

                    for (; i >= 0; i--)
                    {
                        timeStep = updates[i];
                        timeStep.Update(spanTime);
                        updates.Update(i, timeStep);
                    }
                }
                pointSecond += UnityEngine.Time.deltaTime;
            }
        }
    }
}
