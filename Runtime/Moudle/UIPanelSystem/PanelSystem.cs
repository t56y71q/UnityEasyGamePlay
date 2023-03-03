using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public class PanelSystem
    {
        private Stack<Panel> panels = new Stack<Panel>();

        public void Show(Panel panel)
        {
            if (panels.Count > 0)
                panels.Peek().Freeze();
            panels.Push(panel);
            panel.SetActive(true);
        }

        public void Hide()
        {
            if (panels.Count > 1)
            {
                panels.Pop().SetActive(false);
                panels.Peek().Resume();
            }
            else if (panels.Count > 0)
            {
                panels.Pop().SetActive(false);
            }
        }
    }
}
