using System;
using System.Collections.Generic;
using System.Text;

namespace Poker
{
    class Print  // This whole class is only serving only the purpose of printing to the console
    {
        public List<string> player_cards { get; set; }
        public List<string> board { get; set; }

        public void Show()
        {
            Console.OutputEncoding = Encoding.UTF8;
            int num_players = player_cards.Count / 2;

            string player_name = "    PLAYER_";  // this is the Player's Names line
            string rivals_message = "";
            for (int i = 2; i < num_players + 1; i++)
            {
                player_name += Convert.ToString(i) + "    ";
                rivals_message += player_name;
                player_name = "    PLAYER_";
            }

            Console.WriteLine("\n\nYOUR CARDS ARE:\n");  // we create our print like this because its faster
            string cards_mine = "  {0}---{0}  {2}---{2}\n" +
                "  | {1} |  | {3} |\n" +
                "  |{1}{1}{1}|  |{3}{3}{3}|\n" +
                "  | {1} |  | {3} |\n" +
                "  {0}---{0}  {2}---{2}\n\n";
            Console.WriteLine(cards_mine, player_cards[0][0], player_cards[0][1], player_cards[1][0], player_cards[1][1]);
            player_cards.RemoveAt(0);
            player_cards.RemoveAt(0);            

            Console.WriteLine("THE BOARD IS:\n");  // same with the board
            string cards_board = "  {0}---{0}  {2}---{2}  {4}---{4}  {6}---{6}  {8}---{8}\n" +
                "  | {1} |  | {3} |  | {5} |  | {7} |  | {9} |\n" +
                "  |{1}{1}{1}|  |{3}{3}{3}|  |{5}{5}{5}|  |{7}{7}{7}|  |{9}{9}{9}|\n" +
                "  | {1} |  | {3} |  | {5} |  | {7} |  | {9} |\n" +
                "  {0}---{0}  {2}---{2}  {4}---{4}  {6}---{6}  {8}---{8}\n\n";
            Console.WriteLine(cards_board, board[0][0], board[0][1], board[1][0], board[1][1],
                board[2][0], board[2][1], board[3][0], board[3][1], board[4][0], board[4][1]);

            string first_line = "";
            for (int i = 0; i < (num_players - 1)*2 -1; i += 2)  // 1st and 5th lines (card number)
            {
                first_line += ($"  {player_cards[i][0]}---{player_cards[i][0]}  {player_cards[i + 1][0]}---{player_cards[i + 1][0]}  ");
            }

            string second_line = "";
            for (int i = 0; i < (num_players - 1) * 2 - 1; i += 2)  // 2nd and 4th lines (colors)
            {
                second_line += ($"  | {player_cards[i][1]} |  | {player_cards[i + 1][1]} |  ");
            }

            string third_line = "";
            for (int i = 0; i < (num_players - 1) * 2 - 1; i += 2)  // middle (colors)
            {
                third_line += ($"  |{player_cards[i][1]}{player_cards[i][1]}{player_cards[i][1]}|  |{player_cards[i + 1][1]}{player_cards[i + 1][1]}{player_cards[i + 1][1]}|  ");
            }

            Console.WriteLine("THE OPPONENTS CARDS ARE:\n");  // we print each line, knowing the number of players
            Console.WriteLine(rivals_message);
            Console.WriteLine(first_line);
            Console.WriteLine(second_line);
            Console.WriteLine(third_line);
            Console.WriteLine(second_line);
            Console.WriteLine(first_line);
        }
    }
}