using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private string dbPath;

    void Start()
    {
        string fileName = "Underdeveloped.db";
        string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, fileName);
        string persistentDataPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(persistentDataPath))
        {
            File.Copy(streamingAssetsPath, persistentDataPath);
        }

        dbPath = "URI=file:" + persistentDataPath;
        Debug.Log("We in");

        // using (var connection = new SqliteConnection(dbPath))
        // {
        //     connection.Open();
        //     using (var command = connection.CreateCommand())
        //     {
        //         command.CommandText = "SELECT * FROM Challenges";
        //         using (IDataReader reader = command.ExecuteReader())
        //         {
        //             while (reader.Read())
        //             {
        //                 string columnName = reader["Name"].ToString(); 
        //                 Debug.Log("Column Value: " + columnName);
        //             }
        //         }
        //     }
        //     connection.Close();
        // }
    }

    //Would return [name, dir, testDir]
    public string[] LoadDataViaArea(string area, string level, string status)
    {
        string[] selectedData = new string[3];

        using (SqliteConnection connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Challenges WHERE Level = @level AND Area = @area AND Status = @status LIMIT 1;";
                command.Parameters.AddWithValue("@level", level);
                command.Parameters.AddWithValue("@area", area);
                command.Parameters.AddWithValue("@status", status);

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string directory = reader["Directory"].ToString();
                        string testDir = reader["TestDir"].ToString();

                        selectedData[0] = name;
                        selectedData[1] = directory;
                        selectedData[2] = testDir;

                        Debug.Log("Column Value: " + name);
                    }
                }
            }
            connection.Close();
        }

        return selectedData;
    }

    public void UpdateChallengeStatus(string name, string status)
    {
        using (SqliteConnection connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Challenges SET Status = @status WHERE Name = @name";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@status", status);
                command.ExecuteNonQuery();
            }
            connection.Close();
        }
    }
}
