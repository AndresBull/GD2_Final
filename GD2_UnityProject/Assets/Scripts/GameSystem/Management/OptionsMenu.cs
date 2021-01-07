using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem.Management
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _optionsMenuUI = null;
        [SerializeField] private GameObject _firstSelected = null;
        [SerializeField] private GameObject _newFirstSelected = null;

        public static bool IsGamePaused;
        
        public void AdjustVolume(float sliderValue)
        {
            AudioListener.volume = sliderValue;
        }

        public void CloseOptions()
        {
            _optionsMenuUI.SetActive(false);
            if (GameLoop.Instance.StateMachine.CurrentState is PlayState)
            {
                InputManager.Instance.SwitchToActionMap("Player");
            }
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(_firstSelected);
            Time.timeScale = 1f;
            IsGamePaused = false;
        }

        public void OpenOptions()
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
            _optionsMenuUI.SetActive(true);
            InputManager.Instance.SwitchToActionMap("UI");
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(_newFirstSelected);
        }


        public void ReturnToMenu()
        {
            GameLoop.Instance.StateMachine.MoveTo(GameStates.Menu);
        }
    }
}
