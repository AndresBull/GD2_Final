using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace GameSystem.Management
{
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        internal void SwitchToActionMap(string mapName = "Player")
        {
            switch (mapName)
            {
                case "UI":
                    for (int i = 0; i < PlayerConfigManager.Instance.GetPlayerConfigs().Count; i++)
                    {
                        PlayerConfigManager.Instance.GetPlayerConfigs()[i].Input.SwitchCurrentActionMap(mapName);
                    }
                    break;
                case "Player":
                    for (int i = 0; i < PlayerConfigManager.Instance.GetPlayerConfigs().Count; i++)
                    {
                        var playerConfig = PlayerConfigManager.Instance.GetPlayerConfigs()[i];
                        if (playerConfig.IsOverlord)
                        {
                            playerConfig.Input.SwitchCurrentActionMap("Overlord");
                            continue;
                        }
                        playerConfig.Input.SwitchCurrentActionMap("Climber");
                    }
                    break;
                default:
                    Debug.LogError($"The Action Map \"{mapName}\" does not exist.");
                    break;
            }
        }

        // TODO: REMOVE the following methods if PlayerInput uses BroadcastMessages()
        //       USE the follwing methods if PlayerInput uses Invoke Unity Events
        //public void OnNavigate(InputAction.CallbackContext context)
        //{

        //}

        //public void OnSubmit(InputAction.CallbackContext context)
        //{

        //}

        //public void OnCancel(InputAction.CallbackContext context)
        //{

        //}
    }
}
