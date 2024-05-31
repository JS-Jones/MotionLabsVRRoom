using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour

{
    public bool leftActive = true;
    public bool rightActive = true;
    public int duration = 60;
    public float startingSpeed = 0.1f;
    public string gamemode = "Fixed Positions";
    public string difficulty = "Easy";
    public bool showMarkers = true;
    public float timeThrows = 1f;


    public GameObject startscreen;
    public GameObject optionsscreen;

    public TMP_Text durationText;
    public TMP_Text speedText;
    public TMP_Text gamemodeText;
    public TMP_Text timethrowsText;

    public GameObject subOptions;

    public TMP_Dropdown handChoice;
    public TMP_Dropdown difficultyChoice;
    public TMP_Dropdown markerChoice;

    public void StartPlay()
    {
        optionsscreen.SetActive(true);
        startscreen.SetActive(false);
  
    }

    public void Play()
    {
        PlayerPrefs.SetInt("LeftActive", leftActive ? 1 : 0);
        PlayerPrefs.SetInt("RightActive", rightActive ? 1 : 0);
        PlayerPrefs.SetInt("Duration", duration);
        PlayerPrefs.SetFloat("StartingSpeed", startingSpeed);
        PlayerPrefs.SetString("Gamemode", gamemode);
        PlayerPrefs.SetString("Difficulty", difficulty);
        PlayerPrefs.SetInt("ShowMarkers", showMarkers ? 1 : 0);
        PlayerPrefs.SetFloat("TimeThrows", timeThrows);
        SceneManager.LoadScene(1);
       
    }

    public void HandChoiceHandler()
    {
        if (handChoice.value == 0)
        {
            Debug.Log("both Hands");
            leftActive = true;
            rightActive = true;
        }
        if (handChoice.value == 1)
        {
            leftActive = true;
            rightActive = false;
        }
        if (handChoice.value == 2)
        {
            leftActive = false;
            rightActive = true;
        }
    }
    
    public void DurationChanged()
    {
        //int sampletext = int.Parse(durationText.text);
        //Debug.Log(sampletext);
        //sampletext = sampletext.Trim();
        //string digitsOnly = RemoveNonDigits(sampletext);

        //duration = int.Parse(digitsOnly);
        Debug.Log(duration);
    }

    private string RemoveNonDigits(string s)
    {
        StringBuilder result = new StringBuilder();
        foreach (char c in s)
        {
            if (!char.IsDigit(c))
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }

    public void SpeedChanged(string text)
    {
        string sampletext = speedText.text;
        Debug.Log(sampletext);
        //sampletext = sampletext.Trim();
        //double newtext = double.Parse(sampletext);
        //Debug.Log(newtext);

        //PlayerPrefs.SetFloat("StartingSpeed", float.Parse(sampletext));
        //startingSpeed = float.Parse(sampletext);
        //Debug.Log(PlayerPrefs.GetFloat("StartingSpeed", (float)0.1));

    }

    public void Gamemode(string text)
    {
        string sampletext = gamemodeText.text;

        sampletext = sampletext.Trim();
        
        if (sampletext.Equals("Fixed Positions")){
            subOptions.SetActive(true);
            gamemode = sampletext;

        } 
        if (sampletext.Equals("Free Moving"))
        {
            subOptions.SetActive(false);
            gamemode = sampletext;
        }

    }

    public void DifficultyHandler()
    {
        if (difficultyChoice.value == 0)
        {
            Debug.Log("Easy");
            difficulty = "Easy";
        }
        if (difficultyChoice.value == 1)
        {
            difficulty = "Medium";
        }
        if (difficultyChoice.value == 2)
        {
            difficulty = "Hard";
        }
    }

    public void MarkerHandler()
    {
        if (markerChoice.value == 0)
        {
            Debug.Log("true");
            showMarkers = true;
        }
        if (markerChoice.value == 1)
        {
            showMarkers = false;
        }
    }

    public void TimeThrowsChanged(string text)
    {
        string sampletext = timethrowsText.text;

        sampletext = sampletext.Trim();
        timeThrows = float.Parse(sampletext);
        Debug.Log(timeThrows);

    }
    /* Use if using a button approach
        public void Left()
        {
            leftActive = true; 
            print("leftActive set true");
        }

        public void Right()
        {
            rightActive = true; 
        }

        public void Both()
        {
            leftActive = true; 
            rightActive = true; 
        }*/

}

