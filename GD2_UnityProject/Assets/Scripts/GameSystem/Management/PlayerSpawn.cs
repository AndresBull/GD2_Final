using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameSystem.Management
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _input = null;

        [Header("Character Setup")]
        [Tooltip("A preview of the character without functionality;\nused to update character customization in realtime.")]
        [SerializeField]
        private GameObject _characterView = null;
        [Tooltip("The different player models to choose from for customization.")]
        [SerializeField]
        private Mesh[] _characterMeshes = null;

        [Header("Player Runtime Spawn")]
        [Tooltip("The general Climber object, with all its functionality; used during gameplay.")]
        [SerializeField]
        private GameObject _climberPrefab = null;
        [Tooltip("The Overlord object, with all its functionality; used during gameplay.")]
        [SerializeField]
        private GameObject _overlordPrefab = null;

        private GameObject _character;
        private Transform _playerScreen, _joinText, _colorPicker, _readyButton, _eventSystem;

        private void Awake()
        {
            if (!(GameLoop.Instance.StateMachine.CurrentState is MenuState))
            {
                var spawns = GameObject.Find("PlayerSpawns"); 

                if (spawns != null)
                {
                    SetupPlayerScreen(spawns.transform);
                }
                SceneManager.sceneLoaded += OnSceneLoaded;   
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.IsValid() && scene.isLoaded)
            {
                var spawns = GameObject.Find("PlayerSpawns");

                if (spawns != null)
                {
                    SetupPlayerScreen(spawns.transform);
                    SpawnPlayerInLevel(spawns.transform);
                }
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ResetPlayerScreen();
        }

        private void SetupPlayerScreen(Transform spawns)
        {
            if (GameLoop.Instance.StateMachine.CurrentState is SetupState && _character == null)
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

                _character = Instantiate(_characterView, spawns.transform.GetChild(_input.playerIndex));
            }
        }

        private void SpawnPlayerInLevel(Transform spawns)
        {
            if (!(GameLoop.Instance.StateMachine.CurrentState is PlayState))
                return;

            if (PlayerConfigManager.Instance.GetPlayerConfigs()[_input.playerIndex].IsOverlord)
            {
                var overlordSpawn = GameObject.Find("OverlordSpawn").transform;
                GameObject overlord = Instantiate(_overlordPrefab, overlordSpawn.position, overlordSpawn.rotation);
                overlord.transform.SetParent(transform);
                return;
            }
            GameObject climber = Instantiate(_climberPrefab, spawns.GetChild(_input.playerIndex).position, spawns.GetChild(_input.playerIndex).rotation, transform);
            climber.transform.SetParent(transform);
        }

        private void ResetPlayerScreen()
        {
            if (GameLoop.Instance.StateMachine.CurrentState is SetupState)
            {
                _eventSystem.GetComponent<MultiplayerEventSystem>().sendNavigationEvents = false;
                _input.uiInputModule = null;
                _readyButton.GetComponent<Button>().interactable = false;
                _colorPicker.GetComponent<Button>().interactable = false;
                _joinText.gameObject.SetActive(true);
                _character = null;
            }
        }
    }
}