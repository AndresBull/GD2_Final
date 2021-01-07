using SoundSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameSystem.Management
{
    public class MenuState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
        }

        protected sealed override void SetupScene()
        {
            if (Time.time >= Time.deltaTime)
            {
                SceneManager.LoadScene("MainMenu");
                InputManager.Instance.SwitchToActionMap("UI");
                PlayerConfigManager.Instance.RemovePlayers(1);
            }
            SoundManager.Instance.PlayTitle();
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
            PlayerConfigManager.Instance.NextOverlordIndex = UnityEngine.Random.Range(0, PlayerConfigManager.Instance.GetPlayerConfigs().Count);
        }

        protected sealed override void SetupScene()
        {
            SceneManager.LoadScene("CharacterSetup");
            InputManager.Instance.SwitchToActionMap("UI");
            SoundManager.Instance.PlayOverworld();
        }
    }

    public class RulesState : BaseState
    {
        protected override void CleanUpScene()
        {
        }

        protected override void SetupScene()
        {
            SceneManager.LoadScene("Rules");
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
            PlayerConfigManager.Instance.SetPlayerAsOverlord();
            InputManager.Instance.SwitchToActionMap("Player");
            SoundManager.Instance.InPlayScene = true;
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

    public class GameOverState : BaseState
    {
        protected override void CleanUpScene()
        {
            
        }

        protected override void SetupScene()
        {
            SceneManager.LoadScene("HighScoreScreen");
            InputManager.Instance.SwitchToActionMap("UI");
            SoundManager.Instance.PlayEndGame();
        }
    }

    public class EndState : BaseState
    {
        protected sealed override void CleanUpScene()
        {
            SoundManager.Instance.StopCurrentlyPlaying();
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
