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
        public override void OnEnter()
        {
            if (Time.time >= Time.deltaTime) // prevent loading the scene on startup and creating an infinite loop
            {
                SceneManager.LoadScene("MainMenu");
            }
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void SetupScene()
        {
            InputManager.Instance.SwitchToActionMap("UI");
        }
    }

    public class SetupState : BaseState
    {
        public override void OnEnter()
        {
            SceneManager.LoadScene("CharacterSetup");
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void SetupScene()
        {
            InputManager.Instance.SwitchToActionMap("UI");
        }
    }

    public class PlayState : BaseState
    {
        public override void OnEnter()
        {
            SceneManager.LoadScene("Combination");
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void SetupScene()
        {
            PlayerConfigManager.Instance.SetPlayerAsOverlord(UnityEngine.Random.Range(0, PlayerConfigManager.Instance.GetPlayerConfigs().Count));
            InputManager.Instance.SwitchToActionMap("Player");
        }
    }

    public class RoundOverState : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void SetupScene()
        {
            InputManager.Instance.SwitchToActionMap("UI");
        }
    }

}
