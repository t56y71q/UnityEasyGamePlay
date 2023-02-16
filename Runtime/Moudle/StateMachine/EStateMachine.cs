using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class EStateMachine:ITick
    {
        private List<ITick> states ;
        private Queue<ITick> adds;
        private Queue<ITick> removes;
        private Queue<StateMachine> changeStates = new Queue<StateMachine>(queueCount);
        private bool isTicking;
        private bool isOverdrive;

        public const int queueCount = 32;

        internal void Init()
        {
            states = new List<ITick>(queueCount);
            adds = new Queue<ITick>(queueCount);
            removes = new Queue<ITick>(queueCount);
            changeStates = new Queue<StateMachine>(queueCount);
        }

        public void AddStateMachine(StateMachine stateMachine)
        {
            changeStates.Enqueue(stateMachine);
            if (!isTicking)
            {
                FrameWork.frameWork.AddTick(this);
                isTicking = true;
            }
        }

        internal void AddStateUpdate(ITick tick)
        {
            adds.Enqueue(tick);
            if (!isTicking)
            {
                isTicking = true;
                FrameWork.frameWork.AddTick(this);
            }
        }

        internal void RemoveStateUpdate(ITick tick)
        {
            removes.Enqueue(tick);
        }

        internal void Destroy()
        {
            if (isTicking)
                FrameWork.frameWork.RemoveTick(this);
        }

        public void Tick()
        {
            while (adds.Count > 0)
            {
                states.Add(adds.Dequeue());
            }

            while (removes.Count > 0)
            {
                states.Remove(removes.Dequeue());
            }

            if (states.Count == 0 && changeStates.Count == 0)
            {
                isTicking = false;
                FrameWork.frameWork.RemoveTick(this);
                return;
            }

            for (int i = 0; i < states.Count; i++)
            {
                states[i].Tick();
            }

            while (changeStates.Count > 0)
            {
                changeStates.Dequeue().Update();
            }

            int length = changeStates.Count;
            if (length < queueCount)
            {
                isOverdrive = true;
                for (int i = length; i > 0; i--)
                {
                    changeStates.Dequeue().Update();
                }
            }
            else
            {
                for (int i = queueCount; i > 0; i--)
                {
                    changeStates.Dequeue().Update();
                }
            }

            if (changeStates.Count == 0 && isOverdrive)
            {
                isOverdrive = false;
                changeStates = new Queue<StateMachine>(queueCount);
            }
        }
    }
}
