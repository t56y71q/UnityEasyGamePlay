using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class EasyGamePlayEntry:MonoBehaviour
    {
       [GameAttribute]public string gameClass;
       [SerializeField] private GameConfig gameConfig;

        private void Awake()
        {
            if (FrameWork.frameWork == null)
            {
                FrameWork.CreateFrameWork();
            }
            
            FrameWork.frameWork.CreateGame(gameClass,gameConfig);
        }
    }

}
