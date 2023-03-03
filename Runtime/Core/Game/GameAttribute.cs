using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGamePlay
{
    [AttributeUsage(AttributeTargets.Field)]
    public class GameAttribute:PropertyAttribute
    {
        public int index;
    }
}
