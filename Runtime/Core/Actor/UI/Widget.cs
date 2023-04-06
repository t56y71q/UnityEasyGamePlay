using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Widget : Actor
    {
        protected CanvasGroup canvasGroup;

        protected override void OnAwake()
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        protected override void OnDisable()
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = false;
        }

        protected override void OnEnable()
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
