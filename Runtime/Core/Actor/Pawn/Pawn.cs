using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class Pawn:Actor
    {
        public Controller controller { get => mController; set { mController = value; mController.Process(this); } }

        private Controller mController;

        protected override void OnDestroy()
        {
            controller?.UnProcess();
        }
    }
}
