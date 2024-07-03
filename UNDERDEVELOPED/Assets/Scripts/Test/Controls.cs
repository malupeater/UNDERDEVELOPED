using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameObject editor, console;
    void Start()
    {
        editor.SetActive(true);
        console.SetActive(false);
    }

    void Update()
    {
        
    }

    public void editorClick()
    {
        editor.SetActive(true);
        console.SetActive(false);
    }

    public void consoleClick()
    {
        editor.SetActive(false);
        console.SetActive(true);
        console.transform.GetChild(0).gameObject.SetActive(true);
    }
}
