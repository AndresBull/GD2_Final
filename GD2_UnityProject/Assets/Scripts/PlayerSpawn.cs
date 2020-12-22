using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject _characterView;
    [Tooltip("The different player models to choose from.")][SerializeField]
    private Mesh[] _characterMeshes;
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private PlayerInput _input;

    private GameObject _character;
    private Transform _playerScreen, _joinText, _colorPicker, _readyButton, _eventSystem;

    private void Awake()
    {
        var spawns = GameObject.Find("PlayerSpawns");

        if (GameLoop.Instance.GameState == GameState.CharacterSetup && _character == null)
        {
            SetupPlayerScreen();
            _character = Instantiate(_characterView, spawns.transform.GetChild(_input.playerIndex));
        }
        else if (GameLoop.Instance.GameState == GameState.Play)
        {
            Instantiate(_playerPrefab, spawns.transform.GetChild(_input.playerIndex));
        }
    }

    private void SetupPlayerScreen()
    {
        PlayerConfigManager.Instance.SetPlayerMeshes(_input.playerIndex, _characterMeshes);
        var menu = GameObject.Find("PlayerMenu");

        _playerScreen = menu.transform.GetChild(_input.playerIndex);
        _playerScreen.GetComponent<PlayerScreenUpdater>().SetPlayerIndex(_input.playerIndex);
        _playerScreen.GetComponent<PlayerScreenUpdater>().SwitchCharacter();
        
        _joinText = _playerScreen.Find("CharacterPicker").Find("JoinText");
        _joinText.gameObject.SetActive(false);

        _colorPicker = _playerScreen.Find("ColorPicker");
        _colorPicker.GetComponent<Button>().interactable = true;

        _readyButton = _playerScreen.Find("ReadyButton");
        _readyButton.GetComponent<Button>().interactable = true;

        _eventSystem = _playerScreen.GetChild(_playerScreen.childCount - 1);
        _input.uiInputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
        _eventSystem.GetComponent<MultiplayerEventSystem>().sendNavigationEvents = true;
    }

    private void OnDestroy()
    {
        Destroy(_character);
        ResetPlayerScreen();
    }

    private void ResetPlayerScreen()
    {
        _eventSystem.GetComponent<MultiplayerEventSystem>().sendNavigationEvents = false;
        _input.uiInputModule = null;
        _readyButton.GetComponent<Button>().interactable = false;
        _colorPicker.GetComponent<Button>().interactable = false;
        _joinText.gameObject.SetActive(true);

    }
}
