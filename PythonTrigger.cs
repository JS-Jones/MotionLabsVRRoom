using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonTrigger : MonoBehaviour
{
    public RunPythonScript scriptRunner;

    void Start()
    {
        scriptRunner = GetComponent<RunPythonScript>();
    }

    void Update()
    {
        // Example: Run script on key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Pressed Space");
            scriptRunner.PrintString("Hello from Unity!");
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Pressed Up Arrow");
            scriptRunner.Increase();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Pressed Down Arrow");
            scriptRunner.Decrease();
        }
    }
}
