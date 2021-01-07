using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Management
{
    public class HighScoreDataPanel : MonoBehaviour
    {
        private List<PlayerConfiguration> _playerRankingsList;

        [SerializeField]
        private GameObject _UIPrefab = null;


        private void Awake()
        {
            //Create rankings
            _playerRankingsList = PlayerConfigManager.Instance.GetPlayerConfigs();

            for (int i = 0; i < _playerRankingsList.Count; i++)
            {
                for (int j = 0; j < _playerRankingsList.Count; j++)
                {
                    if (_playerRankingsList[j].RoundScore < _playerRankingsList[i].RoundScore)
                    {
                        var temp = _playerRankingsList[i];
                        _playerRankingsList[i] = _playerRankingsList[j];
                        _playerRankingsList[j] = temp;
                    }
                }
            }



            //Spawn instances of screen
            for (int i = 0; i < _playerRankingsList.Count; i++)
            {
                var config = _playerRankingsList[i];
                var go = Instantiate(_UIPrefab, transform.position, Quaternion.identity, transform);
                go.GetComponent<HighScorePanel>().SetRankingText(i + 1, config.PlayerIndex);
                go.GetComponent<HighScorePanel>().SetScoreText(config.RoundScore);
            }
        }
    }
}
