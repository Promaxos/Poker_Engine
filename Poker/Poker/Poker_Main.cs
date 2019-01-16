using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    class Poker_Main
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<string> rankNames = new List<string>
            {
                "Straight Flush",
                "Four Of A Kind",
                "Full House",
                "Flush",
                "Straight",
                "Set",
                "Two Pair",
                "Pair",
                "High Card"};

            while (true)
            {
                Deck deck = new Deck();
                string card1;
                string card2;
                while (true)
                {
                    Console.WriteLine("Please choose your first card, (for example, 2H stands for 2 of Hearts, TC stands for Ten of Clubs).");
                    card1 = Console.ReadLine();
                    if (card1.Length == 2)
                    {
                        card1 = card1.ToUpper();
                        string el_color1 = card1.Substring(1);
                        if (el_color1 == "H")
                        {
                            card1 = card1.Remove(1) + "♥";
                        }
                        else if (el_color1 == "C")
                        {
                            card1 = card1.Remove(1) + "♣";
                        }
                        else if (el_color1 == "S")
                        {
                            card1 = card1.Remove(1) + "♠";
                        }
                        else if (el_color1 == "D")
                        {
                            card1 = card1.Remove(1) + "♦";
                        }
                        if (deck.InDeck(card1))
                            break;
                        else
                        {
                            Console.WriteLine("Wrong Input!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input!\n");
                    }
                }
                while (true)
                {
                    Console.WriteLine("Please choose your second card.");
                    card2 = Console.ReadLine();
                    if (card2.Length == 2)
                    {
                        card2 = card2.ToUpper();
                        string el_color2 = card2.Substring(1);
                        if (el_color2 == "H")
                        {
                            card2 = card2.Remove(1) + "♥";
                        }
                        else if (el_color2 == "C")
                        {
                            card2 = card2.Remove(1) + "♣";
                        }
                        else if (el_color2 == "S")
                        {
                            card2 = card2.Remove(1) + "♠";
                        }
                        else if (el_color2 == "D")
                        {
                            card2 = card2.Remove(1) + "♦";
                        }
                        if (deck.InDeck(card2))
                            break;
                        else
                        {
                            Console.WriteLine("Wrong Input!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input!\n");
                    }
                }
                int num_players;
                while (true)
                {
                    Console.WriteLine("Please choose the number of players. (between 2 and 9)");
                    string input_number = Console.ReadLine();
                    bool result = int.TryParse(input_number, out num_players);
                    if (result && 2 <= num_players && num_players <= 9)
                        break;
                }
                List<string> player_cards = new List<string> { card1, card2 };  // we keep track of all the players' hands for the combination class
                                                                                // the first two cards are our cards
                List<string> final_ranking = new List<string>();  // we add here the final ranking of each player's hand

                for (int i = 0; i < (num_players - 1) * 2; i++)
                {
                    string card = deck.DrawCard();
                    player_cards.Add(card);
                }

                List<string> the_board = deck.GetBoard();

                Print pr = new Print();
                pr.board = the_board;
                pr.player_cards = player_cards;
                pr.Show();
                Console.WriteLine();




                Combination comb = new Combination();
                comb.board = the_board;
                comb.card_1 = card1;
                comb.card_2 = card2;
                final_ranking.Add(comb.WhichCombo());

                for (int i = 0; i < num_players - 1; i++)
                {
                    comb.card_1 = player_cards[0];
                    player_cards.Remove(player_cards[0]);
                    comb.card_2 = player_cards[0];
                    player_cards.Remove(player_cards[0]);
                    final_ranking.Add(comb.WhichCombo());
                }
                        
                List<string> final_ranking_sorted = new List<string>(final_ranking);  // deciding the rank name and the winning player
                final_ranking_sorted.Sort();

                int win_count = 0;
                string winner = final_ranking_sorted[0];  // the winning combo (we will iterate it to find if we split)
                foreach (string a in final_ranking)
                {
                    if (a == final_ranking_sorted[0])  // we count the number of people who have the winning combo, in case of split
                    {
                        win_count++;
                    }
                }

                string winner_rank = rankNames[Convert.ToInt32(final_ranking[0][0]) - '0' - 1];  // the winning rank

                if (win_count > 1)
                {
                    List<int> winners_indexes = new List<int>();
                    for (int i = 0; i < final_ranking.Count; i++)
                    {
                        if (final_ranking[i] == winner)
                        {
                            winners_indexes.Add(i);
                        }
                    }
                    string line = "";
                    for (int i = 0; i < win_count; i++)
                    {
                        line += ($"Player {winners_indexes[i] + 1} * ");
                    }
                    Console.WriteLine($"It's a split between {line}!!\nThey all have {winner_rank}!!!");
                }
                else
                {
                    winner_rank = rankNames[Convert.ToInt32(final_ranking_sorted[0][0]) - '0' - 1];
                    Console.WriteLine($"Player {final_ranking.IndexOf(winner) + 1} wins the game with {winner_rank}!!!");
                }

                string answer;
                while (true)
                {
                    Console.WriteLine("\nDo you want to play again? (Y/N)");
                    answer = Console.ReadLine();
                    if (answer.ToUpper() == "Y" || answer.ToUpper() == "N")
                        break;
                    else
                    {
                        Console.WriteLine("Please answer with (Y)es or (N)o!!");
                    }
                }
                if (answer.ToUpper() == "N")
                    break;
                else
                {
                    Console.WriteLine("Let's go again!\n");
                }
            }
        }
    }
}