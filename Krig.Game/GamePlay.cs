﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krig.Game
{
    public class GamePlay
    {
        private List<Card> deck1, deck2, swapPile1, swapPile2, warWinnings;
        private int turnCounter;
        private Random random = new Random();
        int player1AdjustSize, player2AdjustSize;

        public GamePlay() { }

        public void newGame()
        {
            generateDecks();
            deck1 = shuffleDeck(deck1);
            deck2 = shuffleDeck(deck2);
            //Setup swap piles
            swapPile1 = new List<Card>();
            swapPile2 = new List<Card>();
            warWinnings = new List<Card>();
            turnCounter = 1;
        }

        public void loadGame(SaveGameData savedGame)
        {
            turnCounter = savedGame.NumberOfTurns;
            deck1 = savedGame.Player1Deck;
            deck2 = savedGame.Player2Deck;
            swapPile1 = savedGame.Player1Swap;
            swapPile2 = savedGame.Player2Swap;
            warWinnings = new List<Card>();
        }

        public SaveGameData prepareSave(SaveGameData data)
        {
            data.NumberOfTurns = turnCounter;
            data.Player1Deck = deck1;
            data.Player1Swap = swapPile1;
            data.Player2Deck = deck2;
            data.Player2Swap = swapPile2;
            Console.WriteLine("Saving");
            SaveLoad save = new SaveLoad(data);
            return data;
        }

        public Boolean gameOver()
        {
            if (deck1.Count == 0 && swapPile1.Count == 0)
            {
                Console.WriteLine("Player 1 looses!");
                return true;
            }
            else if (deck2.Count == 0 && swapPile2.Count == 0)
            {
                Console.WriteLine("Player 2 looses!");
                return true;
            }
            else
            {
                return false;
            }
        }

        public int playRound(Card player1Card, Card player2Card)
        {
            Console.WriteLine("Turn " + turnCounter + " begins");

            //Declare war
            if (player1Card.Value == player2Card.Value)
            {
                Console.WriteLine("War declared");
                return 0;
            }
            //Player 1 wins
            else if (player1Card.Value > player2Card.Value)
            {
                Console.WriteLine("Player 1 wins round");
                swapPile1.Add(player1Card);
                swapPile1.Add(player2Card);

            }
            //Player 2 wins
            else
            {
                Console.WriteLine("Player 2 wins round");
                swapPile2.Add(player1Card);
                swapPile2.Add(player2Card);

            }
            Console.WriteLine("Deck sizes: deck1: " + deck1.Count + " deck2: " + deck2.Count);
            Console.WriteLine("Swap sizes: swap1: " + swapPile1.Count + " swap2: " + swapPile2.Count);

            //Check if a player has run out of cards
            if (deck1.Count == 0)
            {
                Console.WriteLine("Player 1 uses swap");
                deck1 = shuffleDeck(swapPile1);
                Console.WriteLine("Deck 1 size after swap: " + deck1.Count);
            }
            if (deck2.Count == 0)
            {
                Console.WriteLine("Player 2 uses swap");
                deck2 = shuffleDeck(swapPile2);
                Console.WriteLine("Deck 2 size after swap: " + deck2.Count);
            }

            turnCounter++;
            int cardsInPlay = deck1.Count + deck2.Count + swapPile1.Count + swapPile2.Count;
            if (cardsInPlay != 52)
            {
                Console.WriteLine("CARD COUNT ERROR: " + cardsInPlay);
                return -1;
            }
            return 1;
        }

        /**
         * Ensure players has enough cards for war.
         * Otherwise use fewer cards.
         * If player has run out of cards, the game is lost(if 1 or 2 is returned).
        */
        public int checkWarConditions()
        {
            //Check players have enough cards.
            player1AdjustSize = 3;
            player2AdjustSize = 3;
            if (deck1.Count < 4)
            {
                moveCards(deck1, swapPile1);
                deck1 = shuffleDeck(swapPile1);

                if (deck1.Count < 4 && deck1.Count != 0)
                {
                    if (deck1.Count != 0)
                    {
                        Console.WriteLine("Player 1 count exception");
                        player1AdjustSize = deck1.Count;
                    }
                    else
                    {
                        return 1;
                    }
                    
                }
            }
            if (deck2.Count < 4)
            {
                moveCards(deck2, swapPile2);
                deck2 = shuffleDeck(swapPile2);

                if (deck2.Count < 4)
                {
                    if (deck2.Count != 0)
                    {
                        Console.WriteLine("Player 2 count exception");
                        player2AdjustSize = deck2.Count;
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            return 0;
        }

        /**
         * Handles war condition
         * Returns true if player1(Human) wins
        */
        public int war(List<Card> player1Cards, List<Card> player2Cards, int playerChoice)
        {
            //Add cards to war array
            moveCards(player1Cards, warWinnings);
            moveCards(player2Cards, warWinnings);
            

            Console.WriteLine("Player 1 has cards");
            for (int i = 0; i < player1Cards.Count; i++)
            {
                Console.WriteLine(player1Cards[i].ToString());
            }
            Console.WriteLine("Player 2 has cards");
            for (int i = 0; i < player2Cards.Count; i++)
            {
                Console.WriteLine(player2Cards[i].ToString());
            }

            //Select card
            Card player1Card = (Card)player1Cards[playerChoice];
            Console.WriteLine("Player 1 picks: " + player1Card.ToString());

            int player2Pick = random.Next(0, player2AdjustSize - 1);
            Card player2Card = (Card)player2Cards[player2Pick];
            Console.WriteLine("Player 2 picks: " + player2Card.ToString());

            //Player 1 wins
            if (player1Card.Value > player2Card.Value)
            {
                moveCards(warWinnings, swapPile1);
                return 1;
            }
            //Player 2 wins
            else if (player2Card.Value > player1Card.Value)
            {
                moveCards(warWinnings, swapPile2);
                return 2;
            }
            else
            {
                //Recursive call - need to call war again.
                return 0;
            }
        }

        public Card drawACard()
        {
            return drawSingleCard(deck1);
        }

        public Card getAICard()
        {
            return drawSingleCard(deck2);
        }

        public List<Card> getWarCards()
        {
            return drawCards(deck1, player1AdjustSize);
        }

        public List<Card> getAIWarCards()
        {
            return drawCards(deck2, player2AdjustSize);
        }

        private Card drawSingleCard(List<Card> deck)
        {
            Card card = deck.FirstOrDefault();
            deck.Remove(card);
            return card;
        }

        /**
         * Draws cards from a deck. 
        */
        private List<Card> drawCards(List<Card> deck, int numberOfCards)
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < numberOfCards; i++)
            {
                cards.Add(deck[0]);
                deck.RemoveAt(0);
            }
            return cards;
        }

        /**
         * Moves cards from one array to another.
        */
        private void moveCards(List<Card> from, List<Card> to)
        {
            foreach (var item in from)
            {
                to.Add(item);
                from.Remove(item);
            }

            //for (int i = 0; i < from.Count; i++)
            //{
            //    Card c = (Card)from[i];
            //    to.Add(c);
            //    from.Remove(i);
            //}
        }

        //Generates cards and creates a black and a red deck.
        private void generateDecks()
        {
            deck1 = new List<Card>();
            deck2 = new List<Card>();
            //Generate cards.
            Card card;
            //Loop though suits
            for (int s = 0; s < 4; s++)
            {
                String suit = "";
                int color = -1;
                switch (s)
                {
                    case 0:
                        suit = "spades";
                        color = 0;
                        break;
                    case 1:
                        suit = "clubs";
                        color = 0;
                        break;
                    case 2:
                        suit = "hearts";
                        color = 1;
                        break;
                    case 3:
                        suit = "diamonds";
                        color = 1;
                        break;
                    default:
                        Console.WriteLine("We got problems");
                        break;
                }
                //Loop though values
                for (int i = 1; i <= 13; i++)
                {
                    card = new Card(i, color, suit);
                    if (color == 0)
                    {
                        deck1.Add(card);
                    }
                    else
                    {
                        deck2.Add(card);
                    }
                }
            }
        }

        private List<Card> shuffleDeck(List<Card> deck)
        {
            List<Card> shuffledDeck = new List<Card>();

            while (deck.Count > 0)
            {
                int randomIndex = random.Next(0, deck.Count - 1);
                shuffledDeck.Add(deck[randomIndex]);
                deck.RemoveAt(randomIndex);
            }
            Console.WriteLine("Deck size: " + shuffledDeck.Count);
            return shuffledDeck;
        }
    }
}
