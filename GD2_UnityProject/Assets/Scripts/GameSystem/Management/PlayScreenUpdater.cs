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
        [SerializeField] private Image _ladderImage = null, _lightImage = null;
        [SerializeField] private Sprite _redLight = null, _greenLight = null;

        private int _playerIndex;

        private void Awake()
        {
            PlayerConfigManager.Instance.OnScoreChanged += OnScoreChanged;
            PlayerConfigManager.Instance.OnLadderEquipChanged += OnLadderEquipChanged;
            PlayerConfigManager.Instance.OnSpecialChanged += OnSpecialChanged;
            _ladderImage.gameObject.SetActive(true);
        }
        private void OnDestroy()
        {
            PlayerConfigManager.Instance.OnScoreChanged -= OnScoreChanged;
            PlayerConfigManager.Instance.OnLadderEquipChanged -= OnLadderEquipChanged;
            PlayerConfigManager.Instance.OnSpecialChanged -= OnSpecialChanged;
        }
        public void SetPlayerIndex(int index)
        {
            _playerIndex = index;
            _playerName.SetText($"Player {_playerIndex + 1}");
            _scoreText.text =PlayerConfigManager.Instance.GetPlayerConfigs()[_playerIndex].RoundScore.ToString("0000");
        }

        private void OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            if (e.PlayerIndex != _playerIndex)
                return;

            _scoreText.text = e.NewScore.ToString("0000");
        }

        private void OnLadderEquipChanged(object sender, LadderEquipChangedEventArgs e)
        {
            if (e.PlayerIndex != _playerIndex)
                return;
            
            //_ladderImage.gameObject.SetActive(e.IsCarryingLadder);
        }

        private void OnSpecialChanged(object sender, SpecialChangedEventArgs e)
        {
            if (e.PlayerIndex != _playerIndex)
                return;

            if (e.CanUseSpecial)
                _lightImage.sprite = _greenLight;
            else
                _lightImage.sprite = _redLight;
        }

    }
}
