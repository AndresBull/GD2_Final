using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem.Management
{
    public class StartState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
        }

        protected sealed override void SetupScene()
        {
            // prevent loading the scene on startup and creating an infinite loop
            if (Time.time >= Time.deltaTime)
            {
                SceneManager.LoadScene("Start");
                PlayerConfigManager.Instance.RemovePlayers();
                InputManager.Instance.SwitchToActionMap("UI");
            }
        }
    }

    public class MenuState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
        }

        protected sealed override void SetupScene()
        {
            SceneManager.LoadScene("MainMenu");
            InputManager.Instance.SwitchToActionMap("UI");
            PlayerConfigManager.Instance.RemovePlayers(1);
        }
    }

    public class SetupState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
            foreach (PlayerConfiguration config in PlayerConfigManager.Instance.GetPlayerConfigs())
            {
                config.Input.gameObject.GetComponent<PlayerSpawn>().ResetPlayerScreen();
            }
        }

        protected sealed override void SetupScene()
        {
            SceneManager.LoadScene("CharacterSetup");
            InputManager.Instance.SwitchToActionMap("UI");
        }
    }

    public class PlayState : BaseState
    {
        public event EventHandler OnPlayStateEntered;

        protected sealed override void CleanUpScene()
        {
            PlayerConfigManager.Instance.DestroyConfigChildren();
        }

        protected sealed override void SetupScene()
        {
            OnPlayStateEntered?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene("Play");
            PlayerConfigManager.Instance.SetPlayerAsOverlord(UnityEngine.Random.Range(0, PlayerConfigManager.Instance.GetPlayerConfigs().Count));
            InputManager.Instance.SwitchToActionMap("Player");
        }
    }

    public class RoundOverState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
        }

        protected sealed override void SetupScene()
        {
            InputManager.Instance.SwitchToActionMap("UI");
        }
    }

    public class EndState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
            GameLoop.Destroy(PlayerConfigManager.Instance.gameObject);
            GameLoop.Destroy(SoundManager.Instance.gameObject);
            GameLoop.Destroy(GameLoop.Instance.gameObject);
        }

        protected sealed override void SetupScene()
        {
            CleanUpScene();
        }
    }

}
