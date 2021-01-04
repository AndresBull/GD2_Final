using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointSystemScript
{
    private static int pointsAddedOnPlayerKill = 1000;


    public static void PlayerGotKilled()
    {
        int overLordIndex =  PlayerConfigManager.Instance.GetOverLordIndex();

        PlayerConfigManager.Instance.UpdatePlayerRoundScore(overLordIndex, pointsAddedOnPlayerKill);
    }


}
