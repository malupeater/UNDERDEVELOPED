using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Subsystems;

public class QuestController : MonoBehaviour
{
    public GameObject editor;
    string challenge = "", folderPath = "Assets/Challenges/South";
    void Start()
    {
        if(!Directory.Exists(folderPath))
        {
            Debug.Log("Gay");
           return;
        }

        string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
        
        foreach(string txtFile in txtFiles)
        {
            using (StreamReader reader = new StreamReader(txtFile))
            {
                string line;
                int currentLine = 0;
                while((line = reader.ReadLine()) != null)
                {
                    if(currentLine == 0)
                    {
                        //setTitle
                        currentLine++;
                        continue;
                    }
                    challenge += line + "\n";
                    Debug.Log(challenge);
                    currentLine++;
                }
            }
            
        }
    }

    void Update()
    {
        
    }

    public void LoadQuest()
    {
        editor.GetComponent<TMP_InputField>().text = challenge;
    }
}
