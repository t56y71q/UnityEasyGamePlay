using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    public static class RectTransformEx
    {
        

        public static void SetAnchor(this RectTransform rectTransform, AncherPoint ancherPoint)
        {
            switch(ancherPoint)
            {
                case AncherPoint.LeftUp:
                    rectTransform.anchorMin = Vector2.up;
                    rectTransform.anchorMax = Vector2.up;
                    break;
                case AncherPoint.Up:
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    break;
                case AncherPoint.RightUp:
                    rectTransform.anchorMin = Vector2.one;
                    rectTransform.anchorMax = Vector2.one;
                    break;
                case AncherPoint.Left:
                    rectTransform.anchorMin = Vector2.up / 2;
                    rectTransform.anchorMax = Vector2.up / 2;
                    break;
                case AncherPoint.Center:
                    rectTransform.anchorMin = Vector2.one / 2;
                    rectTransform.anchorMax = Vector2.one / 2;
                    break;
                case AncherPoint.Right:
                    rectTransform.anchorMin = new Vector2(1, 0.5f);
                    rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case AncherPoint.LeftBottom:
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.zero;
                    break;
                case AncherPoint.Bottom:
                    rectTransform.anchorMin = Vector2.right / 2;
                    rectTransform.anchorMax = Vector2.right / 2;
                    break;
                case AncherPoint.RightBottom:
                    rectTransform.anchorMin = Vector2.right;
                    rectTransform.anchorMax = Vector2.right;
                    break;
                case AncherPoint.FullScreen:
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    break;
            }
        }
    }

    public enum AncherPoint
    {
        LeftUp,
        Up,
        RightUp,

        Left,
        Center,
        Right,

        LeftBottom,
        Bottom,
        RightBottom,

        FullScreen
    }
}
