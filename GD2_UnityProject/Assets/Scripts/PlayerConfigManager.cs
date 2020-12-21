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

    [SerializeField]
    private int MaxPlayers = 4;

    [SerializeField]
    private int PlayersReadyBeforeStarting = 3;

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
            _playerConfigs.RemoveAt(input.playerIndex);
        }
    }

    public void SetPlayerCharacter(int playerIndex, GameObject character)
    {
        _playerConfigs[playerIndex].Character = character;
    }

    public void SetPlayerColor(int playerIndex, Material color)
    {
        _playerConfigs[playerIndex].PlayerMaterial = color;
    }

    public void ReadyPlayer(int playerIndex)
    {
        _playerConfigs[playerIndex].IsReady = true;

        if (_playerConfigs.Count == PlayersReadyBeforeStarting && _playerConfigs.All(p => p.IsReady == true))
        {
            SetTimer(15.0f);
        }
        else if (_playerConfigs.Count == MaxPlayers && _playerConfigs.All(p => p.IsReady == true))
        {
            SetTimer(5.0f);
        }
    }

    public void UnreadyPlayer(int playerIndex)
    {
        _playerConfigs[playerIndex].IsReady = false;

        if (_playerConfigs.Count(p => p.IsReady == true) < PlayersReadyBeforeStarting)
        {
            StopTimer();
        }
        else if (_playerConfigs.Count(p => p.IsReady == true) < MaxPlayers)
        {
            StopTimer();
            SetTimer(15.0f);
        }
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
    public GameObject Character { get; set; }
    public int PlayerIndex { get; internal set; }
    public bool IsReady { get; set; }
}
