using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TMP_InputField editor;
    public DatabaseManager databaseManager;
    private string[] currentQuest;
    private string playerArea, challenge;
    private int playerAttempts;
    
    void Start()
    {
        playerAttempts = 0;
    }

    //fetch all valid challenges from the database and returns only one randomly 
    public string[] RetrieveChallengeViaAreaAndStatus(string area, string status)
    {
        string[][] retrievedChallenges = databaseManager.LoadAllChallengesViaAreaAndStatus(area, status);
        int randomNum = Random.Range(0, retrievedChallenges.Length - 1);
        return retrievedChallenges[randomNum];
    }

    public void RetrieveRandomEasyChallenge()
    {
       currentQuest = RetrieveChallengeViaAreaAndStatus(playerArea, "Easy");
    }

    public void RetrieveRandomMidChallenge()
    {
        currentQuest = RetrieveChallengeViaAreaAndStatus(playerArea, "Mid");
    }

    public void RetrieveRandomHardChallenge()
    {
        currentQuest = RetrieveChallengeViaAreaAndStatus(playerArea, "Hard");
    }

    public string[] GetCurrentQuest()
    {
        return this.currentQuest;
    }

    public void LoadChallengeToConsole()
    {
        RetrieveRandomEasyChallenge(); //remove after
        string filePath = Path.Combine(Application.streamingAssetsPath, Path.Combine(currentQuest[3], currentQuest[0] + ".txt"));
        challenge = ReadChallengeTxt(filePath);
        editor.text = challenge;
    }

    public string ReadChallengeTxt(string filePath)
    {
        string challengeTxt = "";

        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                challengeTxt += line + "\n";
            }
        }

        return challengeTxt;
    }

    public string PlayerSolveChallenge(int numOfFailedTest)
    {
        bool playerPass  = numOfFailedTest > 0? false: true;

        if (!playerPass)
        {
            return "Failed";
        }

        databaseManager.UpdateChallengeStatus(currentQuest[0], "Done");
        return "Passed";
    }
}
