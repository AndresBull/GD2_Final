using GameSystem.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        [SerializeField] private int _timeBeforeStarting = 15;
        [Tooltip("Time before the round starts when all joined players are ready (in seconds).")]
        [SerializeField] private int _timeBeforeStartingIfReady = 5;

        private List<PlayerConfiguration> _playerConfigs;
        private float _timer = 0.0f;
        private bool _isTimerActive = false;
        public int NextOverlordIndex;

        public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;
        public event EventHandler<LadderEquipChangedEventArgs> OnLadderEquipChanged;
        public event EventHandler<SpecialChangedEventArgs> OnSpecialChanged;
        public event EventHandler<RoundOverEventArgs> OnRoundOver;
        public event EventHandler<TimerEventArgs> OnTimerSet;
        public event EventHandler OnCharacterSpriteChanged;
        public event EventHandler OnCharacterColorChanged;

        private void Awake()
        {
            _playerConfigs = new List<PlayerConfiguration>();
            PlayerInput input = GetComponentInChildren<PlayerInput>();
            PlayerConfiguration config = new PlayerConfiguration(input);
            input.transform.SetParent(transform);
            _playerConfigs.Add(config);
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            foreach (PlayerConfiguration config in _playerConfigs)
            {
                Destroy(config.Input.gameObject);
            }
        }

        internal int GetOverlordIndex()
        {
            foreach (var playerConfig in _playerConfigs)
            {
                if (playerConfig.IsOverlord)
                {
                    return playerConfig.Input.playerIndex;
                }
            }

            return 0;
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


        internal List<PlayerConfiguration> GetAllClimberConfigs()
        {
            List<PlayerConfiguration> configs = new List<PlayerConfiguration>();
            foreach (PlayerConfiguration playerConfig in _playerConfigs)
            {
                if (!playerConfig.IsOverlord)
                {
                    configs.Add(playerConfig);
                }
            }
            return configs;
        }

        internal List<PlayerConfiguration> GetPlayerConfigs()
        {
            return _playerConfigs;
        }

        internal void DisablePlayerCharacters()
        {
            foreach (var config in _playerConfigs)
            {
                if (config.IsOverlord)
                    Destroy(config.Input.gameObject.transform.GetChild(0).gameObject.GetComponent<OverlordHand>());
                else
                    Destroy(config.Input.gameObject.transform.GetChild(0).gameObject.GetComponent<ClimberBehaviour>());
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
                if (_playerConfigs.Count > 1)
                {
                    LeavePlayer(input);
                    return;
                }
            }


            JoinPlayer(input);
        }

        private void LeavePlayer(PlayerInput input)
        {
            if (_playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
            {
                print($"Player {input.playerIndex} Left.");
                var config = _playerConfigs[input.playerIndex];
                _playerConfigs.Remove(config);
                Destroy(config.Input.gameObject);
            }
        }

        internal void OnPlayerLeft(PlayerInput input)
        {
            LeavePlayer(input);
        }

        internal void RemovePlayers(int lastPlayerRemoved = 0)
        {
            if (_playerConfigs.Count - 1 <= lastPlayerRemoved)
                return;

            for (int i = _playerConfigs.Count - 1; i >= lastPlayerRemoved; i--)
            {
                PlayerConfiguration config = _playerConfigs[i];
                LeavePlayer(config.Input);
            }
        }

        internal void SetPlayerAsOverlord()
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
                _playerConfigs[NextOverlordIndex].IsOverlord = true;
                return;
            }
            // TODO: enable this after testing to set an overlord
            if (overlord.Count <= 0)
            {
                _playerConfigs[NextOverlordIndex].IsOverlord = true;
                return;
            }
        }

        internal void SetPlayerCharacter(int playerIndex, GameObject character)
        {
            _playerConfigs[playerIndex].Character = character;
            OnCharacterSpriteChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void SetPlayerSprites(int playerIndex, GameObject[] sprites)
        {
            _playerConfigs[playerIndex].CharacterMeshes = sprites;
        }

        internal void SetPlayerColor(int playerIndex, Color color)
        {
            _playerConfigs[playerIndex].PlayerColor = color;
            OnCharacterColorChanged?.Invoke(this, EventArgs.Empty);
        }

        internal void ToggleLadderEquip(int playerIndex, bool hasLadder)
        {
            OnLadderEquipChanged?.Invoke(this, new LadderEquipChangedEventArgs(playerIndex, hasLadder));
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

        internal void ToggleSpecialUsed(int playerIndex, bool canUseSpecial)
        {
            OnSpecialChanged?.Invoke(this, new SpecialChangedEventArgs(playerIndex, canUseSpecial));
        }

        internal void TriggerRoundOver(int playerIndex)
        {
            OnRoundOver?.Invoke(this, new RoundOverEventArgs(playerIndex));
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
            else if (input.playerIndex != _playerConfigs[0].PlayerIndex)
            {
                LeavePlayer(input);
            }
        }

        private void SetTimer(int timeInSeconds)
        {
            OnTimerSet?.Invoke(this, new TimerEventArgs(true, timeInSeconds));
        }

        private void StopTimer()
        {
            OnTimerSet?.Invoke(this, new TimerEventArgs(false));
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
        internal Color PlayerColor { get; set; }
        internal GameObject Character { get; set; }
        internal GameObject[] CharacterMeshes { get; set; }
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

    public class LadderEquipChangedEventArgs : EventArgs
    {
        public int PlayerIndex;
        public bool IsCarryingLadder;

        public LadderEquipChangedEventArgs(int playerIndex, bool isCarryingLadder)
        {
            PlayerIndex = playerIndex;
            IsCarryingLadder = isCarryingLadder;
        }
    }

    public class SpecialChangedEventArgs : EventArgs
    {
        public int PlayerIndex;
        public bool CanUseSpecial;

        public SpecialChangedEventArgs(int playerIndex, bool canUseSpecial)
        {
            PlayerIndex = playerIndex;
            CanUseSpecial = canUseSpecial;
        }
    }

    public class RoundOverEventArgs : EventArgs
    {
        public int PlayerIndex;

        public RoundOverEventArgs(int playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }

    public class TimerEventArgs : EventArgs
    {
        public int Time;
        public bool IsActivated;

        public TimerEventArgs(bool isActivated, int time = 0)
        {
            IsActivated = isActivated;
            Time = time;
        }
    }
}
