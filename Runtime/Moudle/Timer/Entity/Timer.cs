using System;

namespace EasyGamePlay
{
    public class Timer: IPoolAble
    {
        public bool isLoop { get; set; }
        public float time { get => stopTime; }
        public event Action finish;
        public event Action<float> update;
        private float stopTime;

        internal void SetProperty(float stopTime, Action finish, Action<float> update, bool isLoop = false)
        {
            this.finish = finish;
            this.update = update;
            this.stopTime = stopTime;
            this.isLoop = isLoop;
        }

        internal void Update(float ratio)
        {
            update(ratio);
        }

        internal bool IsUpdateVaild()
        {
            return update != null;
        }

        internal void Finish()
        {
            finish();
        }
    }
}
