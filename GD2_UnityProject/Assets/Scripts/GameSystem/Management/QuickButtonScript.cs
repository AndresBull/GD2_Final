using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.Management;
using System.Security.Cryptography;

public class QuickButtonScript : MonoBehaviour
{
    public void LoadMenu()
    {
        GameLoop.Instance.StateMachine.MoveTo(GameStates.Menu);
    }

}
