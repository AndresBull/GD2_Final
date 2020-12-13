using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwapper : MonoBehaviour
{
    private PlayerInputManager manager;
    public GameObject OverlordPrefab;
    public InputActionReference OverlordReference;
    private void Start()
    {
        manager = gameObject.GetComponent<PlayerInputManager>();
    }
    private void Update()
    {
        if (manager.playerCount == 3)
        {
            manager.playerPrefab = OverlordPrefab;
            manager.joinAction.reference.Set(OverlordReference);
        }
    }
}
