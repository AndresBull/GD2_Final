using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupMenuController : MonoBehaviour
{
    private int _playerIndex;

    [SerializeField]
    private TextMeshProUGUI _playerName;

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private Material _renderTexMaterial;

    public void SetPlayerIndex(int index)
    {
        _playerIndex = index;
        _playerName.SetText($"Player {_playerIndex + 1}");
    }

    public void SetCharacter(GameObject character)
    {
        PlayerConfigManager.Instance.SetPlayerCharacter(_playerIndex, character);
    }

    public void SetColor(Material color)
    {
        PlayerConfigManager.Instance.SetPlayerColor(_playerIndex, color);
    }

    public void ReadyPlayer()
    {
        PlayerConfigManager.Instance.ReadyPlayer(_playerIndex);
        // change button text to ~"unready"
    }

    public void UnreadyPlayer()
    {
        PlayerConfigManager.Instance.UnreadyPlayer(_playerIndex);
        // change button text to "ready!"
    }
}
