using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class GameLoop
    {
        private  UnityTick unityTick;
        private  UnityFixedUpdate unityFixedUpdate;

        public  void Init(GameObject gameObject)
        { 
            unityTick = gameObject.AddComponent<UnityTick>();
            unityFixedUpdate = gameObject.AddComponent<UnityFixedUpdate>();
        }

        public  void AddTick(ITick tick)
        {
            unityTick.AddTick(tick);
        }

        public  void RemoveTick(ITick tick)
        {
            unityTick.RemoveTick(tick);
        }

        public  void NextFrame(Action action)
        {
            unityTick.NextFrame(action);
        }

        public  void AddFixedUpdate(IFixedUpdate fixedUpdate)
        {
            unityFixedUpdate.AddFixedUpdate(fixedUpdate);
        }

        public  void RemoveFixedUpdate(IFixedUpdate fixedUpdate)
        {
            unityFixedUpdate.RemoveFixedUpdate(fixedUpdate);
        }

        public  void Destroy()
        {
           
        }
    }
}
