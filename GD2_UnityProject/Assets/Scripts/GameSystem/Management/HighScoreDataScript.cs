using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.GameSystem.Management
{
    public static class HighScoreDataScript
    {
        static int _amountOfPlayers=0;
        static List<int> _scores = new List<int>();

        public static void SetAmountOfPlayers(int amountOfPlayers)
        {
            _amountOfPlayers = amountOfPlayers;
        }

        public static int GetAmountOfPlayers()
        {
            return _amountOfPlayers;
        }

        public static void SetScoreForPlayer(int score,int playerIndex)
        {
            _scores[playerIndex] = score;
        }

        public static int GetScoreForPlayer(int playerIndex)
        {
            return _scores[playerIndex];
        }
    }
}
