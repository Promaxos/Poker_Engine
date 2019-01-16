using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Combination
    {
        // Properties
        List<string> all_cards = new List<string> { "A", "K", "Q", "J", "T", "9", "8", "7", "6", "5", "4", "3", "2" };  // this is the ranking system
        List<string> all_ranks = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M" };  // for the highcards, from A to M
        Dictionary<string, string> _rank = new Dictionary<string, string> { };

        List<string> only_card = new List<string> { };   // depending on what we need, we have a list with the numbers of the cards
        List<string> only_color = new List<string> { };  // and one with the colors
        public List<string> board { get;  set; }
        public string card_1 { get; set; }
        public string card_2 { get; set; }
        bool straight_flush = false;
        

        public Combination()
        {
            if (_rank.Keys.Count == 0)
            {
                for (int i = 0; i < 13; i++)
                {
                    _rank.Add(all_cards[i], all_ranks[i]);  // we make sure we create the dictionary for the rankings
                }
            }
        }

        public string WhichCombo()    // We are gonna give each combination a score, starting from 1 to 9
        {
            only_card.Clear();
            only_color.Clear();

            if (board.Count == 7)
            {
                board.RemoveAt(board.Count - 1);  // remove the previous' player hand
                board.RemoveAt(board.Count - 1);
                board.Add(card_1);
                board.Add(card_2);  // we add the cards to the board, 7 cards total
            }
            else  // the first case, where only the board exists
            {
                board.Add(card_1);
                board.Add(card_2);
            }

            foreach (string a in board)
            {
                string letter = a.Substring(0, 1);
                string col = a.Substring(1);
                only_card.Add(letter);
                only_color.Add(col);
            }

            List<string> test_only_card = new List<string>(only_card);
            straight_flush = false;
            string combo = StraightFlush(test_only_card, only_color);
            if (combo != "no")  // here we are going to check the final combination of the hand, from strongest to weakest
            {
                return combo;
            }
            combo = FourOfAKind(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = FullHouse(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = Flush(only_card, only_color);
            if (combo != "no")
            {
                return combo;
            }
            straight_flush = false;
            combo = Straight(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = Set(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = TwoPair(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = Pair(only_card);
            if (combo != "no")
            {
                return combo;
            }
            combo = HighCard(only_card);
            return combo;
        }
        
        private string StraightFlush(List<string> cards_only, List<string> color_only)
        {
            List<string> only_card = cards_only;
            List<string> only_color = color_only;
            List<string> flushCheck = FlushForStraightOnly(only_card, only_color);
            if (flushCheck.Count > 0)
            {
                straight_flush = true;
                string straightCheck = Straight(flushCheck);
                if (straightCheck != "no")
                {
                    return straightCheck;
                }
                else
                {
                    return "no";
                }
            }
            else
            {
                return "no";
            }
        }
        private string FourOfAKind(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            int count = 0;
            foreach (string a in only_card)
            {
                count = only_card.Count(item => item == a);  // we check whether a card appears 4 times (4 of a kind)
                if (count > 3)
                {
                    only_card.RemoveAll(item => item == a);
                    only_card = SortHigh(only_card);
                    return "2" + _rank[a] + _rank[only_card[0]];  // example: 2 means FourOfAKind, a compares the same rank, highcard compares for split
                }
            }
            return "no";
        }
        private string FullHouse(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            int count_3 = 0;
            int count_2 = 0;
            List<string> set_cards = new List<string> { };  // there is a possibillity of 2*3 same cards, a set and 2 pairs exc.
            List<string> pair_cards = new List<string> { };
            foreach (string a in only_card)  // for the sets
            {
                count_3 = only_card.Count(item => item == a);
                if (count_3 > 2)
                {
                    set_cards.Add(a);
                }
            }
            set_cards = set_cards.Distinct().ToList();
            if (set_cards.Count < 1)  // if no set, we end it here
            {
                return "no";
            }
            else if (set_cards.Count > 1)  // if 2 sets
            {
                set_cards = set_cards.OrderBy(d => all_cards.IndexOf(d)).ToList();
                return "3" + _rank[set_cards[0]] + _rank[set_cards[1]];
            }

            foreach (string a in only_card)  // for a pair
            {
                count_2 = only_card.Count(item => item == a);
                if (count_2 > 1 && a != set_cards[0])
                {
                    pair_cards.Add(a);
                }
            }

            pair_cards = pair_cards.Distinct().ToList();
            if (pair_cards.Count < 1)  // if no pair, we end it here
            {
                return "no";
            }
            else if (pair_cards.Count == 1)  // if 1 pair
            {
                return "3" + _rank[set_cards[0]] + _rank[pair_cards[0]];
            }
            else  // if 2 pairs
            {
                pair_cards = pair_cards.OrderBy(d => all_cards.IndexOf(d)).ToList();
                return "3" + _rank[set_cards[0]] + _rank[pair_cards[0]];
            }
        }
        private string Flush(List<string> cards_only, List<string> color_only)
        {
            List<string> only_card = cards_only;
            List<string> only_color = color_only;
            List<int> indexes = new List<int> { };  // the indexes of the cards that DO NOT contain the flush color (if it exists)
            int count = 0;
            foreach (string a in only_color)
            {
                count = only_color.Count(item => item == a);  // we check whether a card appears 4 times (4 of a kind)
                
                if (count > 4)
                {
                    foreach (string b in only_color)
                    {
                        if (a != b)
                        {
                            indexes.Add(only_color.IndexOf(b));
                        }
                    }
                    indexes.Reverse();
                    foreach (int c in indexes)  //  we remove the NOT flushed cards from our card list, so we can rank the ones who are flushed
                    {
                        only_card.RemoveAt(c);
                    }
                    only_card = SortHigh(only_card);
                    return "4" + _rank[only_card[0]] + _rank[only_card[1]] + _rank[only_card[2]] + _rank[only_card[3]] + _rank[only_card[4]];
                }
            }
            return "no";
        }
        private string Straight(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            List<int> indexes = new List<int> { };
            only_card = SortHigh(only_card);
            only_card = only_card.Distinct().ToList();
            foreach (string a in only_card)  // we transpose the indexes of the hand depending on card power
            {
                indexes.Add(all_cards.IndexOf(a));
            }
            List<int> correct_indexes = new List<int> { };
            int count = 0;
            for (int i = 1; i < indexes.Count; i++)  // we search whether there are 5 consecutive numbers in the hand
            {
                if (indexes[i] - indexes[i - 1] == 1)
                {
                    count += 1;
                    correct_indexes.Add(indexes[i - 1]);
                    if (count > 3)
                    {
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            if (count > 3)
            {
                string straight_high = all_cards[correct_indexes[0]];  // we pick the first, aka the strongest
                if (straight_flush)
                {
                    return "1" + _rank[straight_high];
                }
                else
                {
                    return "5" + _rank[straight_high];
                }
            }
            else
            {
                return "no";
            }
        }
        private string Set(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            int count = 0;
            foreach (string a in only_card)
            {
                count = only_card.Count(item => item == a);
                if (count > 2)
                {
                    only_card.RemoveAll(item => item == a);
                    only_card = SortHigh(only_card);
                    return "6" + _rank[a] + _rank[only_card[0]] + _rank[only_card[1]];
                }
            }
            return "no";
        }
        private string TwoPair(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            List<string> pair_cards = new List<string> { };
            int count = 0;
            foreach (string a in only_card)
            {
                count = only_card.Count(item => item == a);
                if (count > 1)
                {
                    pair_cards.Add(a);
                }
            }
            pair_cards = pair_cards.Distinct().ToList();
            if (pair_cards.Count > 1)  // if 3 pairs in combo
            {
                pair_cards = SortHigh(pair_cards);
                only_card.RemoveAll(item => item == pair_cards[0]);
                only_card.RemoveAll(item => item == pair_cards[1]);
                only_card = SortHigh(only_card);
                return "7" + _rank[pair_cards[0]] + _rank[pair_cards[1]] + _rank[only_card[0]];
            }
            return "no";
        }
        private string Pair(List<string> cards_only)
        {
            List<string> only_card = cards_only;
            int count = 0;
            foreach (string a in only_card)
            {
                count = only_card.Count(item => item == a);
                if (count > 1)
                {
                    only_card.RemoveAll(item => item == a);  // we return the pair and then the ranking of the 3 highest cards ranked
                    only_card = SortHigh(only_card);
                    return "8" + _rank[a] + _rank[only_card[0]] + _rank[only_card[1]] + _rank[only_card[2]];
                }
            }
            return "no";
        }
        private string HighCard(List<string> cards_only)
        {
            List<string> only_card = cards_only;  // we return all 5 cards
            only_card = SortHigh(only_card);
            return "9" + _rank[only_card[0]] + _rank[only_card[1]] + _rank[only_card[2]] + _rank[only_card[3]] + _rank[only_card[4]];
        }

        private List<string> FlushForStraightOnly(List<string> cards_only, List<string> color_only)
        {
            List<string> only_card = cards_only;
            List<string> only_color = color_only;
            List<int> indexes = new List<int> { };  // the indexes of the cards that DO NOT contain the flush color (if it exists)
            int count = 0;
            foreach (string a in only_color)
            {
                count = only_color.Count(item => item == a);  // we check whether a card appears 4 times (4 of a kind)

                if (count > 4)
                {
                    foreach (string b in only_color)
                    {
                        if (a != b)
                        {
                            indexes.Add(only_color.IndexOf(b));
                        }
                    }
                    indexes.Reverse();
                    foreach (int c in indexes)  //  we remove the NOT flushed cards from our card list, so we can rank the ones who are flushed
                    {
                        only_card.RemoveAt(c);
                    }
                    only_card = SortHigh(only_card);
                    return only_card;
                }
            }
            return new List<string> { };
        }

        private List<string> SortHigh(List<string> el_rest)   // we define which are the remaining high cards worth of returning to player's hand (if need be)
        {
            List<string> rest = el_rest;
            rest = rest.OrderBy(d => all_cards.IndexOf(d)).ToList();
            return rest;
        }
    }
}
/* To explain better, the best combination is decided in the end by sorting the results from this class for each hand.
     * This means that if a player has an output of "2KB", this means the player has the second best combination (2 = Four Of A Kind)
     * The "K" stands for the K"nth" card of the ranking, meaning 4. So the player's Four Of A Kind is on the card 4.
     * Same afterwards, "B" stands for his 5th and last card, the player's high card, which is a King. Player's hand: "4444K"
     * Thinking the same way, "444AQ" is translated into "6KAC", 6->Set's ranking, K->Set on the card "4", A+C->Ace and Queen as highcards.
     */