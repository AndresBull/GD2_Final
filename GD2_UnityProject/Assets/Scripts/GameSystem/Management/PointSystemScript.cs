using GameSystem.Characters;
using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointSystemScript
{
    private static int pointsAddedOnPlayerKill = 1000;
    private static int pointsDeductedBuiltToHigh = 500;

    private static int pointsAddedForLivingThroughAround = 1000;
    private static int pointsAddedForReachingANewHeight=200;


    public static void OverlordBuiltToHigh()
    {
        int overLordIndex = PlayerConfigManager.Instance.GetOverLordIndex();

        PlayerConfigManager.Instance.UpdatePlayerRoundScore(overLordIndex, pointsAddedOnPlayerKill);
    }
    


    public static void GiveAliveClimbersBonus()
    {
        
      List<int> climbers=  PlayerConfigManager.Instance.GetClimberIndexes();

        foreach (int climber in climbers)
        {
            PlayerConfigManager.Instance.UpdatePlayerRoundScore(climber, pointsAddedForLivingThroughAround);
        }
    }

    public static void PlayerGotKilled()
    {
        int overLordIndex =  PlayerConfigManager.Instance.GetOverLordIndex();

        PlayerConfigManager.Instance.UpdatePlayerRoundScore(overLordIndex, pointsAddedOnPlayerKill);
    }

    public static void PlayerReachedNewHeight(int playerIndex, int scoreMultiplier)
    {
        PlayerConfigManager.Instance.UpdatePlayerRoundScore(playerIndex, pointsAddedForReachingANewHeight*scoreMultiplier);
    }

}
