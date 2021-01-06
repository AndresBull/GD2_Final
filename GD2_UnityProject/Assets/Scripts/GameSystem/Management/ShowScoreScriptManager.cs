using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ShowScoreScriptManager
{
    public static ShowScoreScript GetShowScriptWithPlayerIndex(int playerIndex)
    {
        List<PlayerConfiguration> playerConfigs = PlayerConfigManager.Instance.GetPlayerConfigs();
        

        foreach (var player in playerConfigs)
        {

            if (player.PlayerIndex == playerIndex)
                return player.Input.gameObject.transform.GetChild(0).gameObject.GetComponent<ShowScoreScript>();
        }
        return null;
    }
}
