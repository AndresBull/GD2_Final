using Assets.Scripts.GameSystem.Management;
using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHighScoreScript : MonoBehaviour
{
    [SerializeField]
    private GameObject HighScoreUIPrefab;

    PlayerConfiguration _config;

    // Start is called before the first frame update
    void Start()
    {
        int amountOfPlayers = HighScoreDataScript.GetAmountOfPlayers();
        
        for (int i = 0; i < amountOfPlayers; i++)
        {
            GameObject playerInfo = Instantiate(HighScoreUIPrefab);
            GameObject highscorePanel = this.gameObject;

            playerInfo.transform.SetParent(highscorePanel.transform, false);
            playerInfo.GetComponent<PlayScreenUpdater>().SetPlayerIndex(i);

        }

    }
}
