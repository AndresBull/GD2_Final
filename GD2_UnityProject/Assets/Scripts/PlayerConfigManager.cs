using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;

public class PlayerConfigManager : SingletonMonoBehaviour<PlayerConfigManager>
{
    private List<PlayerConfiguration> _playerConfigs;
    private float _timer = 0.0f;
    private bool _isTimerActive = false;

    [Tooltip("The maximum number of players that can join a game session.")][SerializeField]
    private int MaxPlayers = 4;

    [Tooltip("The amount of players that needs to be 'Ready' before the round can start")][SerializeField]
    private int PlayersNeededReadyBeforeStarting = 3;

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return _playerConfigs;
    }

    public List<GameObject> GetClimbers()
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
    }

    private void Update()
    {
        if (!_isTimerActive)
            return;

        if (_timer > 0.0f)
        {
            _timer -= Time.deltaTime;
            return;
        }

        if (_timer <= 0.0f)
        {
            SceneManager.LoadScene("Combination");
        }
    }

    public void OnPlayerJoin(PlayerInput input)
    {
        if (!_playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
        {
            print($"Player {input.playerIndex} Joined.");
            input.transform.SetParent(transform);
            _playerConfigs.Add(new PlayerConfiguration(input));
        }
    }

    public void OnPlayerLeave(PlayerInput input)
    {
        if (_playerConfigs.Any(p => p.PlayerIndex == input.playerIndex))
        {
            print($"Player {input.playerIndex} Left.");
            input.transform.SetParent(null);
            var config = _playerConfigs[input.playerIndex];
            _playerConfigs.Remove(config);
            Destroy(config.Input.gameObject);
        }
    }

    public void SetPlayerRole(int playerIndex, bool isOverlord)
    {
        if (_playerConfigs.Any(pc => pc.IsOverlord))
        {
            _playerConfigs[playerIndex].IsOverlord = false;
            return;
        }

        _playerConfigs[playerIndex].IsOverlord = isOverlord;
    }

    public void SetPlayerCharacter(int playerIndex, Mesh character)
    {
        _playerConfigs[playerIndex].Character = character;
    }

    public void SetPlayerMeshes(int playerIndex, Mesh[] meshes)
    {
        _playerConfigs[playerIndex].CharacterMeshes = meshes;
    }

    public void SetPlayerColor(int playerIndex, Material color)
    {
        _playerConfigs[playerIndex].PlayerMaterial = color;
    }

    public void ToggleReadyPlayer(int playerIndex)
    {
        if (!_playerConfigs[playerIndex].IsReady)
        {
            ReadyPlayer(playerIndex);
            return;
        }
        UnreadyPlayer(playerIndex);
    }

    public void SetTimer(float timeInSeconds)
    {
        _timer = timeInSeconds;
        _isTimerActive = true;
    }

    public void StopTimer()
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

    public PlayerInput Input { get; internal set; }
    public Material PlayerMaterial { get; set; }
    public Mesh Character { get; set; }
    public Mesh[] CharacterMeshes { get; internal set; }
    public int PlayerIndex { get; internal set; }
    public bool IsReady { get; internal set; }
    public bool IsOverlord { get; internal set; }
}
