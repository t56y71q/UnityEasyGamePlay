using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class SubStateMachine : AState, ISubStateMachine
    {
        private StateMachine stateMachine;

        public SubStateMachine()
        {
            stateMachine = new StateMachine();
        }

        public SubStateMachine(int capacity)
        {
            stateMachine = new StateMachine(capacity);
        }

        public void UpdateSubState()
        {
            stateMachine.Update();
        }

        public void AddState(int id, AState aState)
        {
            stateMachine.AddState(id, aState);
        }

        public void AddTransistion(int id, int nextId, Func<bool> func)
        {
            stateMachine.AddTransistion(id, nextId, func);
        }

        public void RemoveTransistion(int id, int nextId)
        {
            stateMachine.RemoveTransistion(id, nextId);
        }

        public void SetDefault(int id)
        {
            stateMachine.SetDefault(id);
        }

        public AState GetCurrentState()
        {
            return stateMachine.GetCurrentState();
        }
    }
}
