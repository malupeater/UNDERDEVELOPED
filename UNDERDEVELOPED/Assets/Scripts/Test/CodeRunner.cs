using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;

public class CodeRunner : MonoBehaviour
{
    public GameObject editor, console;

    private string path;
    private string txt;
    void Start()
    {
        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games/Underdeveloped/ExeFile");
        Mono.createDir(path);
        btnEditor_Click();
    }

    public void runCode()
    {
        if (string.IsNullOrEmpty(editor.GetComponent<TMP_InputField>().text) ||
        string.IsNullOrEmpty(editor.GetComponent<TMP_InputField>().text))
        {
            return;
        }

        if (!checkEntryPoint())
        {
            addEntryPoint();    
        }
        else
        {
            txt = editor.GetComponent<TMP_InputField>().text;
        }

        Mono.createCS(path, "test", txt);
        Mono.compileCS("mcs test.cs", path);

        if(Mono.haveCompilationError())
        {
            string errorMsg = "";
            foreach (string str in Mono.consoleCompileError)
            {
                errorMsg += str;
            }
            console.GetComponent<TextMeshProUGUI>().text = errorMsg;
            return;
        }

        string output = Mono.runExeFile("mono test.exe", path);

        if (Mono.haveRuntimeError())
        {
            string errorMsg = "";
            foreach (string str in Mono.consoleRuntimeError)
            {
                errorMsg += str;
            }
            console.GetComponent<TextMeshProUGUI>().text = errorMsg;
            return;
        }

        console.GetComponent<TextMeshProUGUI>().text = output;
    }

    public void btnEditor_Click()
    {
        editor.SetActive(true);
        console.SetActive(false);
    }

    public void btnConsole_Click()
    {
        editor.SetActive(false);
        console.SetActive(true);
    }

    public void addEntryPoint()
    {
        txt = "using System;\n\n" + 
        "public class ClassA\n" +
        "{\n" +
        "public static void Main(string[] args)\n" +
        "{\n" +
        $"{editor.GetComponent<TMP_InputField>().text}\n" +
        "}\n}";
    }

    public bool checkEntryPoint()
    {
        if (editor.GetComponent<TMP_InputField>().text.Contains("public static void Main(string[] args)") ||
        editor.GetComponent<TMP_InputField>().text.Contains("public static void Main(string[] args){") ||
        editor.GetComponent<TMP_InputField>().text.Contains("static void Main(string[] args){") ||
        editor.GetComponent<TMP_InputField>().text.Contains("static void Main(string[] args)"))
        {
            return true;
        }
        return false;
    }

    public void setEditorCode(string code)
    {
        editor.GetComponent<TMP_InputField>().text = code; 
    }
}
