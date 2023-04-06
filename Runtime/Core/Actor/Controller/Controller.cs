using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class Controller:Actor
    {
        public abstract Pawn pawn { get; set; }

    }

    public abstract class PlayerController : Controller
    {
    }

    public abstract class AiController : Controller
    {
    }
}
