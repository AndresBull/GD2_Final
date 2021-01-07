using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameSystem.Management
{
    public class HighScorePanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _rankingText = null;

        [SerializeField]
        private TextMeshProUGUI _scoreText = null;

        public void SetRankingText(int placement, int playerIndex)
        {
            if (placement == 1)
                _rankingText.text = $"First place - Player {playerIndex}!";
            else if(placement == 2)
                _rankingText.text = $"Second place - Player {playerIndex}!";
            else if (placement == 3)
                _rankingText.text = $"Third place - Player {playerIndex}!";
            else if (placement >= 4)
                _rankingText.text = $"Fourth place - Player {playerIndex}!";

        }

        public void SetScoreText(int score)
        {
            _scoreText.text = $"Final score - {score}";
        }

    }
}
