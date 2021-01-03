﻿using System;
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
        [Tooltip("The maximum number of players that can join a game session.")]
        [SerializeField] private int _maxPlayers = 4;
        [Tooltip("The amount of players that needs to be 'Ready' before the round can start")]
        [SerializeField] private int _playersNeededReadyBeforeStarting = 3;
        [Tooltip("Time before the round starts when not all joined players are ready yet (in seconds).")]
        [SerializeField] private float _timeBeforeStarting = 15f;
        [Tooltip("Time before the round starts when all joined players are ready (in seconds).")]
        [SerializeField] private float _timeBeforeStartingIfReady = 5f;

        private List<PlayerConfiguration> _playerConfigs;
        private float _timer = 0.0f;
        private bool _isTimerActive = false;

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;
        public event EventHandler OnCharacterMeshChanged;
        public event EventHandler OnCharacterMaterialChanged;

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
                // prevent the scene from continuous loading
                _isTimerActive = false;
                GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
            }
        }

        private void OnDestroy()
        {
            foreach (PlayerConfiguration config in _playerConfigs)
            {
                Destroy(config.Input.gameObject);
            }
        }

        internal List<GameObject> GetAllClimbers()
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

        internal List<PlayerConfiguration> GetPlayerConfigs()
        {
            return _playerConfigs;
        }

        internal void RemovePlayers(int lastPlayerRemoved = 0)
        {
            if (_playerConfigs.Count - 1 <= lastPlayerRemoved)
                return;

            for (int i = _playerConfigs.Count - 1; i >= lastPlayerRemoved; i++)
            {
                PlayerConfiguration config = _playerConfigs[i];
                OnPlayerLeft(config.Input);
            }
        }
        
        internal void DestroyConfigChildren()
        {
            foreach (var config in _playerConfigs)
                Destroy(config.Input.gameObject.transform.GetChild(0).gameObject);
        }

        internal void OnPlayerJoined(PlayerInput input)
        {
            if (!(GameLoop.Instance.StateMachine.CurrentState is SetupState))
            {
                if (_playerConfigs.Count >= 1)
                {
                    return;
                }
                JoinPlayer(input);
                GameLoop.Instance.StateMachine.MoveTo(GameStates.Menu);
                return;
            }
            JoinPlayer(input);
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
            OnCharacterMeshChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void SetPlayerMeshes(int playerIndex, Mesh[] meshes)
        {
            _playerConfigs[playerIndex].CharacterMeshes = meshes;
        }

        internal void SetPlayerColor(int playerIndex, Material color)
        {
            _playerConfigs[playerIndex].PlayerMaterial = color;
            OnCharacterMaterialChanged?.Invoke(this, EventArgs.Empty);
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

        internal void UpdatePlayerRoundScore(int playerIndex, int addedScore)
        {
            int newScore = _playerConfigs[playerIndex].RoundScore + addedScore;
            _playerConfigs[playerIndex].RoundScore = newScore;

            OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs(playerIndex, newScore));
        }

        internal void ResetPlayerRoundScore(int playerIndex)
        {
            _playerConfigs[playerIndex].RoundScore = 0;
        }

        internal void UpdatePlayerTotalScore(int playerIndex)
        {
            _playerConfigs[playerIndex].TotalScore += _playerConfigs[playerIndex].RoundScore;
        }

        private void JoinPlayer(PlayerInput input)
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

            if (_playerConfigs.Count >= _playersNeededReadyBeforeStarting 
                && _playerConfigs.Count < _maxPlayers && _playerConfigs.All(p => p.IsReady == true))
            {
                SetTimer(_timeBeforeStarting);
            }

            else if (_playerConfigs.Count == _maxPlayers && _playerConfigs.All(p => p.IsReady == true))
            {
                SetTimer(_timeBeforeStartingIfReady);
            }
        }

        private void UnreadyPlayer(int playerIndex)
        {
            _playerConfigs[playerIndex].IsReady = false;

            if (_playerConfigs.Count(p => p.IsReady == true) < _playersNeededReadyBeforeStarting)
            {
                StopTimer();
            }
            else if (_playerConfigs.Count(p => p.IsReady == true) < _maxPlayers)
            {
                StopTimer();
                SetTimer(_timeBeforeStarting);
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
        internal int RoundScore { get; set; }
        internal int TotalScore { get; set; }
        internal bool IsReady { get; set; }
        internal bool IsOverlord { get; set; }
    }

    public class ScoreChangedEventArgs : EventArgs
    {
        public int PlayerIndex;
        public int NewScore;

        public ScoreChangedEventArgs(int playerIndex, int newScore)
        {
            PlayerIndex = playerIndex;
            NewScore = newScore;
        }
    }
}
