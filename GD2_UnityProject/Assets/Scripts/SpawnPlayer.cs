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
    [SerializeField]
    private Transform _playerSpawn;

    private void Awake()
    {
        var root = GameObject.Find("Menu");
        if (root != null)
        {
            var menu = Instantiate(_playerPrefab, _playerSpawn);
            _input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<SetupMenuController>().SetPlayerIndex(_input.playerIndex);
        }

    }
}
