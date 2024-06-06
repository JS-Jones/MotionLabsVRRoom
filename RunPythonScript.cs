using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RunPythonScript : MonoBehaviour
{
    private string pythonPath = @"/Users/Awesomekids/opt/anaconda3/bin/python3.9"; // Path to your python executable
    private string scriptPath = @"/Users/Awesomekids/Desktop/PythonUnityTest/scriptv2.py"; // Path to your script.py

    public void RunScript(string command, string[] args = null)
    {
        if (args == null)
        {
            args = new string[] { };
        }

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonPath;
        start.Arguments = string.Format("{0} {1}", scriptPath, string.Join(" ", new string[] { command }.Concat(args)));
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.RedirectStandardError = true;
        start.CreateNoWindow = true;

        using (Process process = Process.Start(start))
        {
            using (System.IO.StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Debug.Log(result);
            }
            using (System.IO.StreamReader reader = process.StandardError)
            {
                string error = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error);
                }
            }
        }
    }

    public void PrintString(string value)
    {
        RunScript("print_string", new string[] { value });
    }

    public void Increase()
    {
        RunScript("increase");
    }

    public void Decrease()
    {
        RunScript("decrease");
    }
}
