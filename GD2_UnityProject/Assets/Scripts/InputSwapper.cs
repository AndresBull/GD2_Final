using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwapper : MonoBehaviour
{
    private PlayerInputManager manager;
    public GameObject ClimberPrefab;
    public InputActionReference ClimberReference;
    private void Start()
    {
        manager = gameObject.GetComponent<PlayerInputManager>();
    }
    private void Update()
    {
        if (manager.playerCount == 1)
        {
            manager.playerPrefab = ClimberPrefab;
            manager.joinAction.reference.Set(ClimberReference);
        }
    }
}
