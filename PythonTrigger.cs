using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PythonTrigger : MonoBehaviour
{
    public RunPythonScript scriptRunner;
    public string speed;

    void Start()
    {
        scriptRunner = GetComponent<RunPythonScript>();

        speed = PlayerPrefs.GetString("StartingSpeed", "0");
    }

    void Update()
    {
        // Example: Run script on key press - incase they want to pause screen.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Emergency Stop");
            scriptRunner.Stop();
        }

    }

    // if the speed is greater than 0 then call the pytonctorller to start or stop.
    public void Starter()
    {
        if (float.Parse(speed) > 0)
        {
            scriptRunner.Move(speed);
        }
    }

    public void Stopper()
    {
        if (float.Parse(speed) > 0)
        {
            scriptRunner.Move("0");
        }
    }
}