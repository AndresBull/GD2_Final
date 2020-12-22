using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPlayerScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _playerPrefabs;

    [SerializeField]
    private TextMeshProUGUI _playerName;

    [SerializeField]
    private Button _readyButton;

    [SerializeField]
    private Button _colorPicker;

    private Mesh[] _playerMeshes;
    private int _playerIndex;
    private int _index;

    private void Awake()
    {
        _playerMeshes = new Mesh[_playerPrefabs.Length];
        for (int i = 0; i < _playerPrefabs.Length; i++)
        {
            _playerMeshes[i] = _playerPrefabs[i].GetComponentInChildren<MeshFilter>().sharedMesh;
        }
    }

    public void SetPlayerIndex(int index)
    {
        _playerIndex = index;
        _playerName.SetText($"Player {_playerIndex + 1}");
    }

    public void SwitchCharacter()
    {
        print(_index);
        if (_index < _playerMeshes.Length - 1)
        {
            _index++;
        }
        else
        {
            _index = 0;
        }

        SetCharacter(_playerMeshes[_index]);
    }

    public void ToggleReadyStatus()
    {
        PlayerConfigManager.Instance.ToggleReadyPlayer(_playerIndex);

        if (_readyButton.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Ready!"))
        {
            _readyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Unready");
            return;
        }
        _readyButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Ready!");
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
