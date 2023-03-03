using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Hud:Panel
    {
        protected override void OnAwake()
        {
            if(!gameObject.TryGetComponent<Canvas>(out Canvas canvas))
            {
                canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            base.OnAwake();
        }
    }
}
