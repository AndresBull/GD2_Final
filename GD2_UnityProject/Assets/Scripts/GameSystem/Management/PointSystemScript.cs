using GameSystem.Characters;
using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class PointSystemScript
{
    private static int pointsForPlayerKill = 1000;
    private static int pointsForBuildingTooHigh = -500;

    private static int pointsForLivingThroughARound = 1000;
    private static int pointsForReachingANewHeight = 200;

    private static int OverlordIndex
    {
        get => PlayerConfigManager.Instance.GetPlayerConfigs().Find(pc => pc.IsOverlord == true).PlayerIndex;
    }

    public static void OverlordBuiltTooHigh()
    {
        PlayerConfigManager.Instance.UpdatePlayerRoundScore(OverlordIndex, pointsForPlayerKill);
    }

    public static void GiveAliveClimbersBonus()
    {
        foreach (PlayerConfiguration config in PlayerConfigManager.Instance.GetAllClimberConfigs())
        {
            PlayerConfigManager.Instance.UpdatePlayerRoundScore(config.PlayerIndex, pointsForLivingThroughARound);
        }
    }

    public static void PlayerGotKilled()
    {
        PlayerConfigManager.Instance.UpdatePlayerRoundScore(OverlordIndex, pointsForPlayerKill);
    }

    public static void PlayerReachedNewHeight(int playerIndex, int scoreMultiplier)
    {
        PlayerConfigManager.Instance.UpdatePlayerRoundScore(playerIndex, pointsForReachingANewHeight * scoreMultiplier);
    }

}
