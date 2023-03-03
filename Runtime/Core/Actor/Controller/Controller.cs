using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class Controller:Actor
    {
        public Pawn pawn { get=> mPawn; }

        private Pawn mPawn;

        public void Process(Pawn pawn)
        {
            this.mPawn = pawn;
        }

        public void UnProcess()
        {
            mPawn = null;
        }
    }

    public abstract class PlayerController : Controller
    {
    }

    public abstract class AiController : Controller
    {
    }
}
