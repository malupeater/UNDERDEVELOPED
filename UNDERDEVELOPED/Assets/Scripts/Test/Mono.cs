using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public static class Mono
{
    public static ArrayList consoleCompileError, consoleRuntimeError;
    public static void ExecuteCommand(string command)
    {
        var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardError = true;
        processInfo.RedirectStandardOutput = true;

        var process = Process.Start(processInfo);

        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
            UnityEngine.Debug.Log("output>>" + e.Data);
        process.BeginOutputReadLine();

        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
            UnityEngine.Debug.Log("error>>" + e.Data);
        process.BeginErrorReadLine();

        process.WaitForExit();

        UnityEngine.Debug.Log($"ExitCode: {process.ExitCode}");
        process.Close();
    }

    public static void compileCS(string command, string dir)
    {
        consoleCompileError = new ArrayList();
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            WorkingDirectory = dir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            UnityEngine.Debug.Log("Command Output:");
            UnityEngine.Debug.Log(output);

            if (!string.IsNullOrWhiteSpace(error))
            {
                consoleCompileError.Add(error);
                UnityEngine.Debug.Log("Command Error:");
                UnityEngine.Debug.Log(error);
            }
        }
    }



    public static void createDir(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }
    public static void createCS(string dir, string name, string text)
    {
        string file = Path.Combine(dir, name+".cs");
        if (!File.Exists(file))
        {
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.Write(text);
            }
        }
        else
        {
            File.Delete(file);
            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.Write(text);
            }
        }
    }

    public static string runExeFile(string command, string dir)
    {
        consoleRuntimeError = new ArrayList();
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command}",
            WorkingDirectory = dir,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(psi))
        {
            StringBuilder outputBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputBuilder.AppendLine(e.Data);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorBuilder.AppendLine(e.Data);
                }
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            string output = outputBuilder.ToString();
            string error = errorBuilder.ToString();

            if (!string.IsNullOrWhiteSpace(error))
            {
                UnityEngine.Debug.Log("Command Error:"); //pls delete me after nigguh
                UnityEngine.Debug.Log(error);
                consoleRuntimeError.Add(error);
            }
            return output;
        }
    }

    public static bool haveCompilationError()
    {
        if(consoleCompileError.Count > 0)
        {
            return true;
        }

        return false;
    }

    public static bool haveRuntimeError()
    {
        if(consoleRuntimeError.Count > 0)
        {
            return true;
        }

        return false;
    }
}
