using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public abstract class Panel : Widget
    {

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
