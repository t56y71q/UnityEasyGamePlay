using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Hud:Widget
    {
        protected override void OnAwake()
        {
            if(!gameObject.TryGetComponent<Canvas>(out Canvas canvas))
            {
                canvas = gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
}
