using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krig.Model;

namespace Krig.Command
{
    class ChooseForWarCommand: IUndoRedoCommand
    {
        private Cards cards;
        private bool isSelected;

        public ChooseForWarCommand(Cards _cards, bool _isSelected )
        {
            cards = _cards;
            isSelected = _isSelected;
        }

        public void Execute()
        {
            cards.IsSelected = isSelected;
        }

        public void UnExecute()
        {
            cards.IsSelected = isSelected;
        }
    }
}
