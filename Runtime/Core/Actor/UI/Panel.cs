using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Panel : Widget
    {

        protected override void OnAwake()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.0f;
            SetActive(false);
        }

        public void Freeze()
        {
            canvasGroup.interactable = false;
        }

        public void Resume()
        {
            canvasGroup.interactable = true;
        }

    }
}
