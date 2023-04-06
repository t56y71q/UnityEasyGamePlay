using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public class StateMachine
    {
        private SortedList<int, AState> states;
        private SortedList<int, List<Transistion>> transistions;
        private AState current;

        public StateMachine()
        {
            states = new SortedList<int, AState>();
            transistions = new SortedList<int, List<Transistion>>();
        }

        public StateMachine(int capacity)
        {
            states = new SortedList<int, AState>(capacity);
            transistions = new SortedList<int, List<Transistion>>();
        }

        public void AddState(int id, AState aState)
        {
            if (!states.ContainsKey(id))
            {
                states.Add(id, aState);
            }
        }

        public void AddTransistion(int id, int nextId, Func<bool> func)
        {
            if (!this.transistions.TryGetValue(id, out List<Transistion> transistions))
            {
                transistions = new List<Transistion>();
                this.transistions.Add(id, transistions);
            }
            transistions.Add(new Transistion(nextId, func));
        }

        public void RemoveTransistion(int id, int nextId)
        {
            if (this.transistions.TryGetValue(id, out List<Transistion> transistions))
            {
                for (int i = 0; i < transistions.Count; i++)
                {
                    if (transistions[i].id == nextId)
                    {
                        transistions.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private void NextState(int id)
        {
            if (states.TryGetValue(id, out AState aState))
            {
                if (current is IStateExit stateExit)
                {
                    stateExit.Exit();
                    if (current is ITick tick)
                        FrameWork.frameWork.stateMachine.RemoveStateUpdate(tick);
                } 

                current = aState;

                if (current is IStateEnter stateEnter)
                {
                    stateEnter.Enter();

                    if (current is ITick tick)
                        FrameWork.frameWork.stateMachine.AddStateUpdate(tick);
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("not find the id:" + id);
            }
        }

        public void SetDefault(int id)
        {
            if (states.TryGetValue(id, out AState aState))
            {
                current = aState;
                if (current is IStateEnter stateEnter)
                {
                    stateEnter.Enter();

                    if (current is ITick tick)
                        FrameWork.frameWork.stateMachine.AddStateUpdate(tick);
                }
                FrameWork.frameWork.stateMachine.AddStateMachine(this);
            }
            else
            {
                UnityEngine.Debug.LogWarning("not find the id:" + id);
            }
        }

        public void Update()
        {
            if (this.transistions.TryGetValue(current.id, out List<Transistion> transistions))
            {
                int i = 0;
                for (; i < transistions.Count && !transistions[i].transistion(); i++) ;
              
                if(i < transistions.Count)
                {
                    FrameWork.frameWork.stateMachine.AddStateMachine(this);
                    NextState(transistions[i].id);
                }
                else if (current is ISubStateMachine subStateMachine)
                {
                    subStateMachine.UpdateSubState();
                }
            }
        }

        public AState GetCurrentState()
        {
            return current;
        }

        struct Transistion
        {
            public int id;
            public Func<bool> transistion;

            public Transistion(int id, Func<bool> transistion)
            {
                this.id = id;
                this.transistion = transistion;
            }
        }
    }
}
