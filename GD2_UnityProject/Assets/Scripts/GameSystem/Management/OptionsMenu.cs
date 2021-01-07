using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Management
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _optionsMenuUI = null;
        
        public static bool IsGamePaused;

        private void Awake()
        {
            InputManager.Instance.OnOptionsOpened += OnOptionsOpened;
            InputManager.Instance.OnOptionsClosed += OnOptionsClosed;
        }
        private void OnDestroy()
        {
            InputManager.Instance.OnOptionsOpened -= OnOptionsOpened;
            InputManager.Instance.OnOptionsClosed -= OnOptionsClosed;
        }

        private void OnOptionsClosed(object sender, EventArgs e)
        {
            _optionsMenuUI.SetActive(false);
            Time.timeScale = 1f;
            IsGamePaused = false;
        }

        private void OnOptionsOpened(object sender, EventArgs e)
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
            _optionsMenuUI.SetActive(true);
        }
    }
}
