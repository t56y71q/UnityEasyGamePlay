using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGamePlay
{
    class GameInitState : AGameSubState
    {
        public GameInitState(AGame game, AGameState gameState) : base(game, gameState)
        {
        }

        public override int id => 0;
    }
}
