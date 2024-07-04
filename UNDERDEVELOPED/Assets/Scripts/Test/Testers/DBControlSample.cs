using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Subsystems;

public class DBControlSample : MonoBehaviour
{
    public GameObject currentDb, userInput;
    public DatabaseManager databaseManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string[] retrivedData = databaseManager.LoadDataViaArea("South", "Easy", "NotDone");
        string output = $"name: {retrivedData[0]} | Directory: {retrivedData[1]} | TestDir: {retrivedData[2]}";


        currentDb.GetComponent<TextMeshProUGUI>().text = "";
        currentDb.GetComponent<TextMeshProUGUI>().text = output;
    }

    public void UpdateStatus()
    {
        string status = userInput.GetComponent<TMP_InputField>().text;

        databaseManager.UpdateChallengeStatus("Example.txt", status);
    }
}
