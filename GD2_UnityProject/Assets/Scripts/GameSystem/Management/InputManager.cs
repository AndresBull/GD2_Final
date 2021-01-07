using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

        public void OnSubmit(Button button)
        {
            if (button == null)
                return;

            switch(button.name)
            {
                case "Button - Play":
                    GameLoop.Instance.StateMachine.MoveTo(GameStates.Setup);
                    break;
                case "Button - Quit":
                    GameLoop.Instance.StateMachine.MoveTo(GameStates.End);
                    break;
                case "Button - Menu":
                    GameLoop.Instance.StateMachine.MoveTo(GameStates.Menu);
                    break;
            }
        }

        public void OnCancel()
        {
            StateMachine<BaseState> stateMachine = GameLoop.Instance.StateMachine;
            BaseState currentState = stateMachine.CurrentState;

            if (currentState is SetupState)
            {
                stateMachine.MoveTo(GameStates.Menu);
            }
        }
    }
}
