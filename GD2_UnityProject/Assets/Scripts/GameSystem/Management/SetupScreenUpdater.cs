﻿using System.Collections;
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

        private Material _color;
        private PlayerConfiguration _config;
        private int _playerIndex;
        private int _index;

        public void SetUpPlayer(int index)
        {
            _config = PlayerConfigManager.Instance.GetPlayerConfigs()[_playerIndex];
            _playerIndex = index;
            _playerName.SetText($"Player {_playerIndex + 1}");
            SetCharacter(_config.CharacterMeshes[0]);
            SetColor(_color);
        }

        public void SwitchCharacter()
        {
            if (_index < _config.CharacterMeshes.Length - 1)
            {
                _index++;
            }
            else
            {
                _index = 0;
            }

            SetCharacter(_config.CharacterMeshes[_index]);
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
