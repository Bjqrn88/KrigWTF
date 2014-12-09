//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krig.Model
{
    public class SaveGameData
    {
        private int numberOfTurns;
        private List<Card> player1Deck, player2Deck, player1Swap, player2Swap;

        /*
         *Serialization requires public parameterless constructor
         */
        public SaveGameData() { }

        public int NumberOfTurns
        {
            get
            {
                return numberOfTurns;
            }
            set
            {
                this.numberOfTurns = value;
            }
        }

        public List<Card> Player1Deck
        {
            get
            {
                return player1Deck;
            }
            set
            {
                this.player1Deck = value;
            }
        }

        public List<Card> Player2Deck
        {
            get
            {
                return player2Deck;
            }
            set
            {
                this.player2Deck = value;
            }
        }

        public List<Card> Player1Swap
        {
            get
            {
                return player1Swap;
            }
            set
            {
                this.player1Swap = value;
            }
        }

        public List<Card> Player2Swap
        {
            get
            {
                return player2Swap;
            }
            set
            {
                this.player2Swap = value;
            }
        }
    }
}
