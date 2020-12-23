using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class InputSwitcher : SingletonMonoBehaviour<InputSwitcher>
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.IsValid() && scene.isLoaded)
        {
            if (GameLoop.Instance.GameState != GameState.Play)
            {
                SwitchToActionMap("UI");
                return;
            }
           


        }
    }

    private void SwitchToActionMap(string mapName = "Climber")
    {
        switch (mapName)
        {
            case "UI":
                for (int i = 0; i < PlayerConfigManager.Instance.GetPlayerConfigs().Count - 1; i++)
                {
                    PlayerConfigManager.Instance.GetPlayerConfigs()[i].Input.SwitchCurrentActionMap(mapName);
                }
                break;
            case "Climber":
                for (int i = 0; i < PlayerConfigManager.Instance.GetPlayerConfigs().Count - 1; i++)
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
}
