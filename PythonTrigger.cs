using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonTrigger : MonoBehaviour
{
    public RunPythonScript scriptRunner;

    void Start()
    {
        scriptRunner = GetComponent<RunPythonScript>();

        string speed = PlayerPrefs.GetString("StartingSpeed", "0");
        scriptRunner.Move(speed);
    }

    void Update()
    {
        // Example: Run script on key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Emergency Stop");
            scriptRunner.Stop();
        }

       /* if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Pressed Down Arrow");
            scriptRunner.Stop();
        }*/
    }
}
