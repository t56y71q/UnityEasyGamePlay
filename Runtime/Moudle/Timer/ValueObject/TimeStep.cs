using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    class TimeStep : IComparable<TimeStep>
    {
        public float time;
        private List<Timer> timers;
        private List<Timer> updateTimers;

        private static Action<Timer> add;
        private static Action<Timer> release;
        private static Action<TimeStep> remove;

        public TimeStep(float time)
        {
            this.time = time;
            timers =null;
            updateTimers = null;
        }

        public TimeStep(Timer timer)
        {
            this.time = timer.time;
            timers = new List<Timer>();
            updateTimers = new List<Timer>();
            if (timer.IsUpdateVaild())
                updateTimers.Add(timer);
            timers.Add(timer);
        }

        public void Init()
        {
            timers = new List<Timer>();
            updateTimers = new List<Timer>();
        }

        public void AddTimer(Timer timer)
        {
            if (timer.IsUpdateVaild())
                updateTimers.Add(timer);
           
            timers.Add(timer);
        }

        public bool RemoveTimer(Timer timer)
        {
            if(timers.Remove(timer))
            {
                if (timer.IsUpdateVaild())
                    updateTimers.Remove(timer);

                release(timer);

                if (timers.Count == 0)
                    remove(this);

                return true;
            }
            return false;
        }

        public void Finish(Queue<Timer> adds)
        {
            Timer timer;
            for (int i=0;i< timers.Count;i++)
            {
                timer = timers[i];
                timer.Finish();

                if (timer.isLoop)
                    adds.Enqueue(timer);
                 else  
                    release(timer);
            }
            timers.Clear();
            updateTimers.Clear();
        }

        public void Update(float deltaTime)
        {
            time -= deltaTime;
            Timer timer;
            for (int i = 0; i < updateTimers.Count; i++)
            {
                timer = updateTimers[i];
                timer.Update(1 - time / timer.time);
            }
        }

        public int CompareTo(TimeStep other)
        {
            return other.time.CompareTo(time);
        }

        public bool Contain(Timer timer)
        {
            int i = 0;
            for (; i < timers.Count && timers[i]!= timer; i++) ;

            return i < timers.Count;
        }

        public int GetTimerCount()
        {
            return timers.Count;
        }

        public static void SetAction(Action<Timer> add, Action<Timer> release, Action<TimeStep> remove)
        {
            TimeStep.add = add;
            TimeStep.release = release;
            TimeStep.remove = remove;
        }
    }
}
