using GameSystem.Characters;
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
        [SerializeField] private PlayerInput _input = null;

        [Header("Character Setup")]
        [Tooltip("A preview of the character without functionality;\nused to update character customization in realtime.")]
        [SerializeField] private GameObject _characterView = null;
        [Tooltip("The different player objects to choose from for customization purposes.")]
        [SerializeField] private GameObject[] _characterObjects = null;
        [Tooltip("The different colors to pick from. Each player gets one of these colors.")]
        [SerializeField] private Color[] _characterColors = null;

        [Header("Player Runtime Spawn")]
        [Tooltip("The general Climber object, with all its functionality; used during gameplay.")]
        [SerializeField] private GameObject _climberPrefab = null;
        [Tooltip("The Overlord object, with all its functionality; used during gameplay.")]
        [SerializeField] private GameObject _overlordPrefab = null;

        [Header("Player UI")]
        [Tooltip("The prefab that holds all UI data of a player.")]
        [SerializeField] private GameObject _playerInfoPrefab = null;

        private GameObject _character;
        private Transform _playerScreen, _joinText, _readyButton, _eventSystem;
        private PlayerConfiguration _config;

        private void Awake()
        {
            if (GameLoop.Instance.StateMachine.CurrentState is SetupState)
            {
                var spawns = GameObject.Find("PlayerSpawns"); 

                if (spawns != null)
                {
                    SetupPlayerScreen(spawns.transform);
                }
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            _config = PlayerConfigManager.Instance.GetPlayerConfigs()[_input.playerIndex];
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
            PlayerConfigManager.Instance.OnCharacterSpriteChanged -= OnCharacterSpriteChanged;
            PlayerConfigManager.Instance.OnCharacterColorChanged -= OnCharacterColorChanged;
            ResetPlayerScreen();
        }

        private void SetupPlayerScreen(Transform spawns)
        {
            if (GameLoop.Instance.StateMachine.CurrentState is SetupState && _character == null)
            {
                PlayerConfigManager.Instance.SetPlayerSprites(_input.playerIndex, _characterObjects);

                GameObject menu = GameObject.Find("PlayerMenu");
                if (_playerScreen == null)
                {
                    _playerScreen = menu.transform.GetChild(_input.playerIndex);
                }
                if (_joinText == null)
                {
                    _joinText = _playerScreen.Find("CharacterPicker").Find("JoinText");
                }
                if (_readyButton == null)
                {
                    _readyButton = _playerScreen.Find("ReadyButton");
                }
                if (_eventSystem == null)
                {
                    _eventSystem = _playerScreen.GetChild(_playerScreen.childCount - 1);
                }

                _playerScreen.GetComponent<SetupScreenUpdater>().SetUpPlayer(_input.playerIndex);
                _joinText.gameObject.SetActive(false);
                _readyButton.GetComponent<Button>().interactable = true;
                _input.uiInputModule = _eventSystem.GetComponent<InputSystemUIInputModule>();
                _eventSystem.GetComponent<MultiplayerEventSystem>().sendNavigationEvents = true;

                InstantiatePlayerPreview(spawns);
            }
        }

        internal void ResetPlayerScreen()
        {
            if (GameLoop.Instance.StateMachine.CurrentState is SetupState)
            {
                _eventSystem.GetComponent<MultiplayerEventSystem>().sendNavigationEvents = false;
                _readyButton.GetComponent<Button>().interactable = false;
                _joinText.gameObject.SetActive(true);
                _character = null;
            }
        }

        private void InstantiatePlayerPreview(Transform spawns)
        {
            PlayerConfigManager.Instance.OnCharacterSpriteChanged += OnCharacterSpriteChanged;
            PlayerConfigManager.Instance.OnCharacterColorChanged += OnCharacterColorChanged;
            PlayerConfigManager.Instance.SetPlayerColor(_input.playerIndex, _characterColors[_input.playerIndex]);
            _character = Instantiate(_characterView, spawns.transform.GetChild(_input.playerIndex));
        }

        private void OnCharacterColorChanged(object sender, EventArgs e)
        {
            if (_character == null)
                return;

            GameObject scarf = GameObject.Find("Scarf");
            GameObject feather = GameObject.Find("Feather");
            scarf.GetComponent<SpriteRenderer>().color = _config.PlayerColor;
            feather.GetComponent<SpriteRenderer>().color = _config.PlayerColor;
        }

        private void OnCharacterSpriteChanged(object sender, EventArgs e)
        {
            if (_character == null)
                return;

            _character = _config.Character;
        }
        

        private void SpawnPlayerInLevel(Transform spawns)
        {
            if (!(GameLoop.Instance.StateMachine.CurrentState is PlayState))
                return;

            GameObject playerInfo = Instantiate(_playerInfoPrefab);
            GameObject infoPanel = GameObject.Find("InfoPanel");
            playerInfo.transform.SetParent(infoPanel.transform, false);
            playerInfo.GetComponent<PlayScreenUpdater>().SetPlayerIndex(_input.playerIndex);

            if (_config.IsOverlord)
            {
                Transform overlordSpawn = GameObject.Find("OverlordSpawn").transform;
                GameObject overlord = Instantiate(_overlordPrefab, overlordSpawn.position, overlordSpawn.rotation);
                overlord.transform.SetParent(transform);
                overlord.GetComponent<OverlordHand>().SetPlayerConfig(_config);
                return;
            }
            GameObject climber = Instantiate(_climberPrefab, spawns.GetChild(_input.playerIndex).position, spawns.GetChild(_input.playerIndex).rotation, transform);
            climber.transform.SetParent(transform);
            climber.GetComponent<ClimberBehaviour>().InitializePlayer(_config);
        }
    }
}