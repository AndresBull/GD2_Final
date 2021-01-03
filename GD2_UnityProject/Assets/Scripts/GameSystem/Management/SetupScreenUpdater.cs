using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Management
{
    public class SetupScreenUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerName = null;
        [SerializeField] private Button _readyButton = null;
        [SerializeField] private Button _colorPicker = null;

        private int _playerIndex;
        private int _index;

        public void SetPlayerIndex(int index)
        {
            _playerIndex = index;
            _playerName.SetText($"Player {_playerIndex + 1}");
        }

        public void SwitchCharacter()
        {
            print(_index);
            if (_index < PlayerConfigManager.Instance.GetPlayerConfigs()[_playerIndex].CharacterMeshes.Length - 1)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }

            SetCharacter(PlayerConfigManager.Instance.GetPlayerConfigs()[_playerIndex].CharacterMeshes[_index]);
        }

        public void ToggleReadyStatus()
        {
            PlayerConfigManager.Instance.ToggleReadyPlayer(_playerIndex);

            if (_readyButton.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Ready"))
            {
                _readyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Cancel");
                return;
            }
            _readyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Ready");
        }

        public void SetColor(Material color)
        {
            PlayerConfigManager.Instance.SetPlayerColor(_playerIndex, color);
        }

        private void SetCharacter(Mesh character)
        {
            PlayerConfigManager.Instance.SetPlayerCharacter(_playerIndex, character);
        }
    }
}
