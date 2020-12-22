using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private PlayerInput _input;

    private void Awake()
    {
        var menu = GameObject.Find("PlayerMenu");
        var playerScreen = menu.transform.GetChild(_input.playerIndex);
        playerScreen.GetComponent<SetupPlayerScreen>().SetPlayerIndex(_input.playerIndex);
        
        var spawns = GameObject.Find("PlayerSpawns");
        Instantiate(_playerPrefab, spawns.transform.GetChild(_input.playerIndex));
    }
}
