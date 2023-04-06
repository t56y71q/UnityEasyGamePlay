using System;

namespace EasyGamePlay
{
    public class Timer: IPoolAble
    {
        public bool isLoop { get; set; }
        public float stopTime { get; set; }
        public event Action finish;
        public event Action<float> update;
        internal float time { get; set; }

        internal static Action<int> remove;
        internal static Action<Timer> release;

        internal void SetProperty(float stopTime, Action finish, Action<float> update, bool isLoop = false)
        {
            this.finish = finish;
            this.update = update;
            this.stopTime = stopTime;
            this.isLoop = isLoop;
            time = 0.0f;
        }

        internal void Update(float deltaTime,int index)
        {
            this.time += deltaTime;
            update?.Invoke(this.time / stopTime);
            if(this.time>stopTime)
            {
                if(isLoop)
                    time = 0.0f;
                else
                {
                    remove(index);
                    release(this);
                }
                    
                finish();
            }
        }

        internal bool IsUpdateVaild()
        {
            return update != null;
        }

    }
}
