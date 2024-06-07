using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RunPythonScript : MonoBehaviour
{
    private string pythonPath = @"C:\Users\Vicon-OEM\AppData\Local\Microsoft\WindowsApps\python.exe"; // Update to the correct path of your python executable
    private string scriptPath = @"C:\Users\Vicon-OEM\Desktop\Motions Lab Test 3D\Motions Lab Test 3D\PythonUnityTest\scriptv2.py"; // Update to your script path


        public void RunScript(string command, string[] args = null)
    {
        if (args == null)
        {
            args = new string[] { };
        }

        string argsString = string.Join(" ", args.Select(arg => $"\"{arg}\""));
        string arguments = $"\"{scriptPath}\" {command} {argsString}";

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pythonPath;
        start.Arguments = arguments;
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

    public void Move(string value)
    {
        RunScript("Move", new string[] { value });
        
    }

    public void Stop()
    {
        RunScript("Stop", new string[] { "0" });
    }
}
