using GameSystem.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GameSystem.Management
{
    class RoundStartingPopup : MonoBehaviour
    {
        [SerializeField] private GameObject _roundStartingPopup = null;
        [SerializeField] private TextMeshProUGUI _roundStartingText = null;

        private void Awake()
        {
            PlayerConfigManager.Instance.OnTimerSet += OnTimerSet;
            _roundStartingPopup.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerConfigManager.Instance.OnTimerSet -= OnTimerSet;
        }

        private void OnTimerSet(object sender, TimerEventArgs e)
        {
            _roundStartingPopup.SetActive(true);

            StopAllCoroutines();
            if (e.Time != 0)
            {
                StartCoroutine(RoundStartingCountdown(e.Time));
            }
        }

        private IEnumerator RoundStartingCountdown(int seconds)
        {
            int secondsToGo = seconds;

            while (secondsToGo >= 0)
            {
                _roundStartingText.text = $"Round starting in \n{secondsToGo}";
                secondsToGo--;
                yield return new WaitForSeconds(1);
            }

            GameLoop.Instance.StateMachine.MoveTo(GameStates.Rules);
            yield break;
        }
    }
}
