using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    public static void SavePlayer(Player player)
    {
        SaveSystem.SavePlayer(player);
    }

    public static PlayerData LoadPlayer()
    {
        PlayerData playerData = SaveSystem.LoadPlayer();

        return playerData;
    }
}
