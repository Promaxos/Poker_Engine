using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Deck
    {
        private Random rnd = new Random();
        List<string> deck = new List<string>();
        List<string> board = new List<string>();

        public Deck()
        {
            char[] numbers = "23456789TJQKA".ToCharArray();  // we create the deck, each card is represented in a form "2♥"
            char[] letters = "CDSH".ToCharArray();
            char[] color = "♣♦♠♥".ToCharArray();
            var deck_fake =
                (from a in numbers
                 from b in color
                 select a.ToString() + b);

            foreach (string a in deck_fake)
            {
                deck.Add(a);
            }
        }

        public bool InDeck(string card)
        {
            if (deck.Contains(card))
            {
                deck.Remove(card);
                return true;
            }
            return false;
        }  // check if card exists in deck

        public List<string> GetBoard()
        {
            for (int i = 0; i < 5; i++)
            {
                int index = rnd.Next(deck.Count);
                string card = deck[index];
                deck.Remove(card);
                board.Add(card);
            }
            return board;
        }  // get a board from deck

        public string DrawCard()
        {
            int index = index = rnd.Next(deck.Count);
            string card = deck[index];
            deck.Remove(card);
            return card;
        }
    }
}
