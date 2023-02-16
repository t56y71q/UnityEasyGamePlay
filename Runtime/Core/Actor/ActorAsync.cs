using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public class ActorAsync
    {
        public event Completed completed;
        public Actor actor { get => mActor; }

        private Actor mActor;

        internal void SetActor(Actor actor)
        {
            mActor = actor;
            completed?.Invoke(actor);
            completed = null;
        }

        public delegate void Completed(Actor actor);
    }

    public class ActorAsync<T> where T:Actor
    {
        public event Completed completed;
        public T actor { get => mActor; }

        private T mActor;

        internal void SetActor(T actor)
        {
            mActor = actor;
            completed?.Invoke(actor);
            completed = null;
        }

        public delegate void Completed(T actor);
    }
}
