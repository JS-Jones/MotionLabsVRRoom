using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel; 
    public bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        // Check for Pause only if the game is not already paused
        if (Input.GetKeyDown(KeyCode.Space) && !isPaused) {
            isPaused = true; 
            Pause(); 
        }

        // Check for Resume only if the game is paused
        else if (Input.GetKeyDown(KeyCode.Space) && isPaused) {
            isPaused = false;
            Resume(); 
        }
    }

    public void Pause() 
    {
        print("pausing the game");
        
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume() 
    {
        print("resuming the game");
        
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
