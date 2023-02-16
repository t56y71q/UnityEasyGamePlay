using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class ActorCreator:MonoBehaviour
    {
        public string type;

        public void Awake()
        {
            if(!string.IsNullOrEmpty(this.type))
            {
                Type type = Type.GetType(this.type);
                if(type!=null)
                {
                    FrameWork.frameWork.world.SpawnActor(type, transform.position, transform.rotation.eulerAngles, transform.localScale);
                }
            }
        }
    }
}
