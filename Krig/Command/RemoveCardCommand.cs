using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krig.Model;

namespace Krig.Command
{
    class RemoveCardCommand : IUndoRedoCommand
    {

        private ObservableCollection<Cards> cards;

        private List<Cards> cardsToRemove;

        public RemoveCardCommand(ObservableCollection<Cards> _cards, List<Cards> _cardsToRemove)
        {
            cards = _cards;
            cardsToRemove = _cardsToRemove;
        }

        public void Execute()
        {
            cardsToRemove.ForEach(x => cards.Remove(x));
        }

        public void UnExecute()
        {
            cardsToRemove.ForEach(x => cards.Add(x));
        }
    }
}
