using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace GameSystem.Management
{
    public class PlayerConfigManager : SingletonMonoBehaviour<PlayerConfigManager>
    {
        private List<PlayerConfiguration> _playerConfigs;
        private float _timer = 0.0f;
        private bool _isTimerActive = false;

        [Tooltip("The maximum number of players that can join a game session.")]
        [SerializeField]
        private int MaxPlayers = 4;

        [Tooltip("The amount of players that needs to be 'Ready' before the round can start")]
        [SerializeField]
        private int PlayersNeededReadyBeforeStarting = 3;

        internal List<PlayerConfiguration> GetPlayerConfigs()
        {
            return _playerConfigs;
        }

        public List<GameObject> GetAllClimbers()
        {
            var climbers = new List<GameObject>();
            foreach (var playerConfig in _playerConfigs)
            {
                if (!playerConfig.IsOverlord)
                {
                    climbers.Add(playerConfig.Input.gameObject.transform.GetChild(0).gameObject);
                }
            }
            return climbers;
        }

        public float Timer => _timer;

        private void Awake()
        {
            _playerConfigs = new List<PlayerConfiguration>();
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!_isTimerActive)
                return;

            if (_timer > 0.0f)
            {
                _timer -= Time.deltaTime;
                print((int)_timer);
                return;
            }

            if (_timer <= 0.0f)
            {
                _isTimerActive = false; // prevent the scene from continuous loading
                GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
            }
        }

        public void OnPlayerJoined(PlayerInput input)
        {
            if (!_playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
            {
                print($"Player {input.playerIndex} Joined.");
                input.transform.SetParent(transform);
                var config = new PlayerConfiguration(input);
                _playerConfigs.Add(config);
            }
            else
            {
                OnPlayerLeft(input);
            }

            if (GameLoop.Instance.StateMachine.CurrentState is MenuState)
            {
                GameLoop.Instance.StateMachine.MoveTo(GameStates.Setup);
            }
        }

        internal void OnPlayerLeft(PlayerInput input)
        {
            if (_playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
            {
                print($"Player {input.playerIndex} Left.");
                var config = _playerConfigs[input.playerIndex];
                _playerConfigs.Remove(config);
                Destroy(config.Input.gameObject);
            }
        }

        internal void SetPlayerAsOverlord(int playerIndex)
        {
            var overlord = _playerConfigs.Where(pc => pc.IsOverlord).ToList();
            
            if (overlord.Count > 1)
            {
                Debug.LogError("There is more than one Overlord active. Please ensure there is always maximum one Overlord active.");
                return;
            }
            if (overlord.Count > 0)
            {
                overlord[0].IsOverlord = false;
                _playerConfigs[playerIndex].IsOverlord = true;
                return;
            }
            if (overlord.Count <= 0)
            {
                _playerConfigs[playerIndex].IsOverlord = true;
                return;
            }
        }

        internal void SetPlayerCharacter(int playerIndex, Mesh character)
        {
            _playerConfigs[playerIndex].Character = character;
        }

        internal void SetPlayerMeshes(int playerIndex, Mesh[] meshes)
        {
            _playerConfigs[playerIndex].CharacterMeshes = meshes;
        }

        internal void SetPlayerColor(int playerIndex, Material color)
        {
            _playerConfigs[playerIndex].PlayerMaterial = color;
        }

        internal void ToggleReadyPlayer(int playerIndex)
        {
            if (!_playerConfigs[playerIndex].IsReady)
            {
                ReadyPlayer(playerIndex);
                return;
            }
            UnreadyPlayer(playerIndex);
        }

        private void SetTimer(float timeInSeconds)
        {
            _timer = timeInSeconds;
            _isTimerActive = true;
        }

        private void StopTimer()
        {
            _isTimerActive = false;
        }

        private void ReadyPlayer(int playerIndex)
        {
            _playerConfigs[playerIndex].IsReady = true;

            if (_playerConfigs.Count == PlayersNeededReadyBeforeStarting && _playerConfigs.All(p => p.IsReady == true))
            {
                SetTimer(15.0f);
            }

            else if (_playerConfigs.Count == MaxPlayers && _playerConfigs.All(p => p.IsReady == true))
            {
                SetTimer(5.0f);
            }
        }

        private void UnreadyPlayer(int playerIndex)
        {
            _playerConfigs[playerIndex].IsReady = false;

            if (_playerConfigs.Count(p => p.IsReady == true) < PlayersNeededReadyBeforeStarting)
            {
                StopTimer();
            }
            else if (_playerConfigs.Count(p => p.IsReady == true) < MaxPlayers)
            {
                StopTimer();
                SetTimer(15.0f);
            }
        }
    }

    public class PlayerConfiguration
    {
        public PlayerConfiguration(PlayerInput input)
        {
            Input = input;
            PlayerIndex = input.playerIndex;
        }

        internal PlayerInput Input { get; private set; }
        internal Material PlayerMaterial { get; set; }
        internal Mesh Character { get; set; }
        internal Mesh[] CharacterMeshes { get; set; }
        internal int PlayerIndex { get; private set; }
        internal bool IsReady { get; set; }
        internal bool IsOverlord { get; set; }
    }
}
