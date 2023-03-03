using System;
using System.Collections.Generic;
using System.Collections;


namespace EasyGamePlay
{
    public abstract class Entity
    {
        public virtual string name { get=> entityName; set=> entityName=value; }
        public bool isActive { get=> active; }

        private string entityName;
        private EntityState entityState;
        private bool active=true;

        internal static Action<Entity> stateUpdate;

        internal void Awake()
        {
            OnAwake();
            stateUpdate(this);
        }

        public void SetActive(bool active)
        {
            if(this.active!= active)
            {
                if(entityState != EntityState.start)
                {
                    if (this.active)
                        OnEnable();
                    else
                        OnDisable();
                }
                this.active = active;
            }
        }

        protected abstract void OnAwake();
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void Destroy();

        internal void Update()
        {
            switch (entityState)
            {
                case EntityState.start:
                    if (active)
                        OnEnable();
                    if (this is IStart start)
                        start.Start();
                    entityState = EntityState.idle;
                    break;
                case EntityState.exit:
                    Destroy();
                    break;
            }
        }

        internal void SetDestroy()
        {
            entityState = EntityState.exit;
            if(active)
            {
                active = false;
                OnDisable();
            }
            stateUpdate(this);
        }

        public static T CreateEntity<T>() where T:Entity,new()
        {
            T entity = new T();
            entity.Awake();
            return entity;
        }

        public static Entity CreateEntity(Type type)
        {
            Entity entity = Activator.CreateInstance(type) as Entity;
            entity.Awake();
            return entity;
        }

        public static void Destroy(Entity entity)
        {
            entity.SetDestroy();
        }

        enum EntityState
        {
            start,
            idle,
            exit
        }
    }
}
