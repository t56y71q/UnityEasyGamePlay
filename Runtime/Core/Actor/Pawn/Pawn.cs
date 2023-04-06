using System;
using System.Collections.Generic;


namespace EasyGamePlay
{
    public abstract class Pawn:Actor
    {
        public Controller owner { get; set; }

        protected override void Destroy()
        {
            if(owner!=null)
            {
                if(owner is AiController aiController)
                {
                    FrameWork.frameWork.world.DestroyActor(aiController);
                }
                else
                {
                    FrameWork.frameWork.world.DestroyPlayerController();
                }
            }
            base.Destroy();
        }
    }
}
