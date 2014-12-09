using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krig.Model;

namespace Krig.Command
{
    class MoveCardCommand : IUndoRedoCommand
    {
        private Cards cards;

        private int beforeX;
        private int beforeY;
        private int afterX;
        private int afterY;

        public MoveCardCommand(Cards _cards, int _beforeX, int _beforeY, int _afterX, int _afterY)
        {
            cards = _cards;
            beforeX = _beforeX;
            beforeY = _beforeY;
            afterX = _afterX;
            afterY = _afterY;
        }

        public void Execute()
        {
            cards.X = afterX;
            cards.Y = afterY;
        }

        public void UnExecute()
        {
            cards.X = beforeX;
            cards.Y = beforeY;
        }
    }
}
