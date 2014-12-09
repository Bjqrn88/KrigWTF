using System.Collections.ObjectModel;
using Krig.Model;

namespace Krig.Command
{
    class DrawCardCommand : IUndoRedoCommand
    {

        private ObservableCollection<Cards> cardsCollection;

        private Cards cards;

        public DrawCardCommand(ObservableCollection<Cards> _observableCollection, Cards  _cards)
        {
            cardsCollection = _observableCollection;
            cards = _cards;
        }

        public void Execute()
        {
            cardsCollection.Add(cards);
        }

        public void UnExecute()
        {
            cardsCollection.Remove(cards);
        }
    }
}
