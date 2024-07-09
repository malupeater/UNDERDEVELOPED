using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;
using UnityEditor.ShaderGraph.Internal;

public class CodeRunner : MonoBehaviour
{
    public GameObject editor, console, status;
    public QuestManager questManager;
    //add reference to another gameObject for test status 

    private string storagePath, codeRunnerPath;
    private string txt;
    void Start()
    {
        storagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games/Underdeveloped/ExeFile");
        MonoCommands.createDir(storagePath);
        btnEditor_Click();

        codeRunnerPath = Path.Combine(Application.streamingAssetsPath, "Scripts/PlayerCodeRunner.txt");
    }

    // public void runCode()
    // {
    //     if (string.IsNullOrEmpty(editor.GetComponent<TMP_InputField>().text) ||
    //     string.IsNullOrEmpty(editor.GetComponent<TMP_InputField>().text))
    //     {
    //         return;
    //     }

    //     if (!checkEntryPoint())
    //     {
    //         addEntryPoint();    
    //     }
    //     else
    //     {
    //         txt = editor.GetComponent<TMP_InputField>().text;
    //     }

    //     MonoCommands.createCS(path, "test", txt);
    //     MonoCommands.compileCS("mcs test.cs", path);

    //     if(MonoCommands.haveCompilationError())
    //     {
    //         string errorMsg = "";
    //         foreach (string str in MonoCommands.consoleCompileError)
    //         {
    //             errorMsg += str;
    //         }
    //         console.GetComponent<TextMeshProUGUI>().text = errorMsg;
    //         return;
    //     }

    //     string output = MonoCommands.runExeFile("mono test.exe", path);

    //     if (MonoCommands.haveRuntimeError())
    //     {
    //         string errorMsg = "";
    //         foreach (string str in MonoCommands.consoleRuntimeError)
    //         {
    //             errorMsg += str;
    //         }
    //         console.GetComponent<TextMeshProUGUI>().text = errorMsg;
    //         return;
    //     }
    //     Debug.Log("Reached the ass");
    //     console.GetComponent<TextMeshProUGUI>().text = output;
    // }

    private string RunCode(string code, string fileName)
    {
        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(code))
        {
            return "";
        }

        // if (!checkEntryPoint())
        // {
        //     addEntryPoint();    
        // }
        // else
        // {
        //     txt = editor.GetComponent<TMP_InputField>().text;
        // }

        MonoCommands.createCS(storagePath, fileName, code);
        MonoCommands.compileCS($"mcs {fileName}.cs", storagePath);

        if(MonoCommands.haveCompilationError())
        {
            string errorMsg = "";
            foreach (string str in MonoCommands.consoleCompileError)
            {
                errorMsg += str;
            }
            return errorMsg;
        }

        string output = MonoCommands.runExeFile($"mono {fileName}.exe", storagePath);

        if (MonoCommands.haveRuntimeError())
        {
            string errorMsg = "";
            foreach (string str in MonoCommands.consoleRuntimeError)
            {
                errorMsg += str;
            }
            return errorMsg;
        }
        Debug.Log("Reached the ass");
        return output;
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

    public void RunPlayerCode()
    {
        //getPlayerCode from editor
        //Add class and entrypoint
        //add function call inside Main()
        //add PlayerFunction
        
        string functionName = questManager.GetCurrentQuest()[0];
        string fileName = "PlayerCode";
        string code = "using System;\n\n" +
        "public class PlayerCode\n" +
        "{\n" +
        "\tpublic static void Main(string[] args)" +
        "\t{\n" +
        "\t\tPlayerCode playerCode = new PlayerCode();\n" +
        $"\t\tplayerCode.{functionName}();\n" +
        "\t}\n\n" +
        $"\t{editor.GetComponent<TMP_InputField>().text}" +
        "}\n";
        string output = "";

        // using (StreamReader reader = new StreamReader(codeRunnerPath))
        // {
        //     string line;

        //     while ((line = reader.ReadLine()) != null)
        //     {
        //         if(line.Contains("//function Call"))
        //         code += line + "\n";
        //     }
        // }
          
        output = RunCode(code, fileName);
        console.GetComponent<TextMeshProUGUI>().text = output;
    }

    public void RunPlayerCodeTest()
    {
        string fileName = "PlayerCodeTest";
        string testTxtPath = Path.Combine(Application.streamingAssetsPath, questManager.GetCurrentQuest()[6]);
        string functionName = questManager.GetCurrentQuest()[0];
        string code = "";
        string output = "";

        using (StreamReader reader = new StreamReader(testTxtPath))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("//function call"))
                {
                    code += line.Replace("//function call", $"playerCodeTest.{questManager.GetCurrentQuest()[0]}();");
                    continue;
                }

                if (line.Contains("//player function"))
                {
                    code += editor.GetComponent<TMP_InputField>().text;
                    continue;
                }
                code += line + "\n";
            }
        }
        
        output = RunCode(code, fileName);
        status.GetComponent<TextMeshProUGUI>().text = output;

        //questManager.PlayerSolveChallenge();
    }
}
