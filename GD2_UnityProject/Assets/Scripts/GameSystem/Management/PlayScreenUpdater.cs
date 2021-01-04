using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Management
{
    public class PlayScreenUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName = null;
        [SerializeField] private TextMeshProUGUI _scoreText = null;
        [SerializeField] private Image _ladderImage = null;

        private int _playerIndex;

        private void Awake()
        {
            PlayerConfigManager.Instance.OnScoreChanged += OnScoreChanged;
            _scoreText.text = "0000";
        }

        public void SetPlayerIndex(int index)
        {
            _playerIndex = index;
            _playerName.SetText($"Player {_playerIndex + 1}");
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (e.PlayerIndex != _playerIndex)
                return;

            _scoreText.text = e.NewScore.ToString("0000");
        }

    }
}
