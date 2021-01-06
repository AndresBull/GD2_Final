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
    class RoundOverPopup : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _roundWinnerText = null;

        [SerializeField]
        private TextMeshProUGUI _countdownText = null;

        private void Awake()
        {
            PlayerConfigManager.Instance.OnRoundOver += OnRoundOver;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            PlayerConfigManager.Instance.OnRoundOver -= OnRoundOver;
        }

        private void OnRoundOver(object sender, RoundOverEventArgs e)
        {
            PlayerConfigManager.Instance.OnRoundOver -= OnRoundOver;

            gameObject.SetActive(true);
            if (e.PlayerIndex >= 0)
            {
                _roundWinnerText.text = $"Round won by: Player {e.PlayerIndex + 1}!";
                PlayerConfigManager.Instance.NextOverlordIndex = e.PlayerIndex;
            }
            else
            {
                _roundWinnerText.text = $"Climbers won this round!";
                var overlordIdx = PlayerConfigManager.Instance.GetOverlordIndex();
                var nextIdx = overlordIdx;
                while (nextIdx == overlordIdx)
                {
                    nextIdx = UnityEngine.Random.Range(0, PlayerConfigManager.Instance.GetPlayerConfigs().Count);
                }
                PlayerConfigManager.Instance.NextOverlordIndex = nextIdx;
            }

            StartCoroutine(EndOfRoundCountdown(10));
        }

        private IEnumerator EndOfRoundCountdown(int seconds)
        {
            int secondsToGo = seconds;

            PlayerConfigManager.Instance.DisablePlayerCharacters();

            while (secondsToGo >= 0)
            {
                _countdownText.text = $"New round in {secondsToGo}";
                secondsToGo -= 1;
                yield return new WaitForSeconds(1);
            }

            GameLoop.Instance.StateMachine.MoveTo(GameStates.Play);
            yield break;
        }

    }
}
