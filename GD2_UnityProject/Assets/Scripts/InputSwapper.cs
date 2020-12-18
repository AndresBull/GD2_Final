using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwapper : MonoBehaviour
{
    private PlayerInputManager manager;
    public GameObject ClimberPrefab;
    public GameObject OverlordPrefab;
    private void Start()
    {
        manager = gameObject.GetComponent<PlayerInputManager>();
    }
    private void Update()
    {
        if (manager.playerCount == 0)
        {
            manager.playerPrefab = OverlordPrefab;
        }
        else
        {
            manager.playerPrefab = ClimberPrefab;
        }
    }
}
