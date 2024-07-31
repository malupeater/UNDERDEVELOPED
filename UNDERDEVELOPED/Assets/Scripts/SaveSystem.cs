using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static void SavePlayer (Player player)
    {
        string playerName = "Test";
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"My Games/Underdeveloped/Saves/{playerName}.plyr");
        
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {            
            PlayerData playerData = new PlayerData(player);
            formatter.Serialize(fileStream, playerData);
        }
    }

    //will return null when theres no file inside the saves folder
    public static PlayerData LoadPlayer()
    {
        string playerName = "Test";
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"My Games/Underdeveloped/Saves/{playerName}.plyr");

        if(!File.Exists(path))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            //return (PlayerData) formatter.Deserialize(fileStream);
            PlayerData playerData = (PlayerData) formatter.Deserialize(fileStream);
            return playerData;
        }
    }
}
