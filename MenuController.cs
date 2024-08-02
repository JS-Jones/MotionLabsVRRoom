using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour

{
    // Sample values that are going to be changing and then assign to playerprefs
    public bool leftActive = true;
    public bool rightActive = true;
    public int duration = 120;
    public string startingSpeed = "0";
    public string gamemode = "Fixed Positions";
    public string throwingSpeed = "Slow";
    public string difficulty = "Easy";
    public int targetCount;
    public bool showMarkers = true;
    public bool repeatMarkers = false;
    public float timeThrows = 5.0f;
    public string inclusivity = "Inclusive";
    public string timerType = "Radial";

    // UI objects with the information
    public GameObject startscreen;
    public GameObject optionsscreen;
    public TMP_InputField durationText;
    public TMP_InputField speedText;
    public TMP_Dropdown throwingSpeedChoice;
    public TMP_Text gamemodeText;
    public TMP_Text timeBetweenThrowsText;
    public TMP_InputField timethrowsText;
    public GameObject subOptions;
    public TMP_Dropdown handChoice;
    public TMP_Dropdown difficultyChoice;
    public TMP_Dropdown inclusivityChoice;
    public TMP_Dropdown markerChoice;
    public TMP_Dropdown repeatChoice;
    public TMP_Dropdown timerTypeChoice;
    public Slider slider;

    private void Start()
    {
        // Difficulty is the target count number, setting it here so its easier to pass
        if (difficulty.Equals("Easy"))
        {
            targetCount = 24;
        } else if (difficulty.Equals("Medium")){
            targetCount = 38;
        }
        else if (difficulty.Equals("Hard"))
        {
            targetCount = 52;
        }
    }

    // Click Play button on the StartScreen
    public void StartPlay()
    {
        optionsscreen.SetActive(true);
        startscreen.SetActive(false);
  
    }

    // Click the Start Game Button on the Options Screen. Pass the values to Player Prefs so that it carries over to a new scene
    public void Play()
    {
        PlayerPrefs.SetInt("LeftActive", leftActive ? 1 : 0);
        PlayerPrefs.SetInt("RightActive", rightActive ? 1 : 0);
        PlayerPrefs.SetInt("Duration", duration);
        //PlayerPrefs.SetInt("ThrowingSpeed", throwingSpeed);
        PlayerPrefs.SetString("ThrowingSpeed", throwingSpeed);
        PlayerPrefs.SetString("StartingSpeed", startingSpeed);
        PlayerPrefs.SetString("TimerType", timerType);
        PlayerPrefs.SetString("Gamemode", gamemode);
        PlayerPrefs.SetString("Difficulty", difficulty);
        PlayerPrefs.SetString("Inclusivity", inclusivity);
        PlayerPrefs.SetInt("ShowMarkers", showMarkers ? 1 : 0);
        PlayerPrefs.SetInt("RepeatMarkers", repeatMarkers ? 1 : 0);
        PlayerPrefs.SetFloat("TimeThrows", timeThrows);
        PlayerPrefs.SetFloat("Weighting", slider.value);
        SceneManager.LoadScene(1);
       
    }

    // Decides which hands should be activated
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
    
    // Changes the duration count, adjusts the time between throws, and also sets to minimum limit if lower than estimated
    public void DurationChanged(TMP_InputField x)
    {

        duration = int.Parse(x.text);

        timeThrows = (float.Parse(x.text) / targetCount);

        // Minimum timebetweenthrows for slow is 3.5, medium = 3, fast = 2.5. Any faster would make the ball not reach the end of the screen.
        if (throwingSpeed.Equals("Slow") && timeThrows < 3.5)
        {
            duration = (int)(3.5 * targetCount);
            x.text = duration.ToString();
            timeThrows = 3.5f;
        } else if (throwingSpeed.Equals("Medium") && timeThrows < 3)
        {
            duration = (int)(3 * targetCount);
            x.text = duration.ToString();
            timeThrows = 3f;
        }
        else if (throwingSpeed.Equals("Fast") && timeThrows < 2.5)
        {
            duration = (int)(2.5 * targetCount);
            x.text = duration.ToString();
            timeThrows = 2.5f;
        }
        timeBetweenThrowsText.text = string.Format("{0:0.##} Seconds", timeThrows);
    }

    //Changes the throwing speed
    public void ThrowingSpeedChanged()
    {

        if (throwingSpeedChoice.value == 0)
        {
            throwingSpeed = "Slow";
        }
        if (throwingSpeedChoice.value == 1)
        {
            throwingSpeed = "Medium";
        }
        if (throwingSpeedChoice.value == 2)
        {
            throwingSpeed = "Fast";
        }
    }

    // changes the clock type
    public void TimerTypeChanged()
    {
     
        if (timerTypeChoice.value == 0)
        {
            timerType = "Radial";
        }
        if (timerTypeChoice.value == 1)
        {
            timerType = "Timed";
        }
        if (timerTypeChoice.value == 2)
        {
            timerType = "None";
        }
    }


    // changes the starting walking speed.
    public void SpeedChanged(TMP_InputField x)
    {
        
        startingSpeed = x.text;
        

    }

    // shows the suboptions of the fixed positions are slected. 
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
    
    // Number of targets handler
    public void DifficultyHandler()
    {
        if (difficultyChoice.value == 0)
        {
            Debug.Log("Easy");
            difficulty = "Easy";
            targetCount = 24;
        }
        if (difficultyChoice.value == 1)
        {
            difficulty = "Medium";
            targetCount = 38;
        }
        if (difficultyChoice.value == 2)
        {
            difficulty = "Hard";
            targetCount = 52;
        }

        // adjusts the duration and time between thows when this changes
        timeThrows = ((float)duration / targetCount);
        if (throwingSpeed.Equals("Slow") && timeThrows < 3.5)
        {
            duration = (int)(3.5 * targetCount);
            durationText.text = duration.ToString();
            timeThrows = 3.5f;
        }
        else if (throwingSpeed.Equals("Medium") && timeThrows < 3)
        {
            duration = (int)(3 * targetCount);
            durationText.text = duration.ToString();
            timeThrows = 3f;
        }
        else if (throwingSpeed.Equals("Fast") && timeThrows < 2.5)
        {
            duration = (int)(2.5 * targetCount);
            durationText.text = duration.ToString();
            timeThrows = 2.5f;
        }
        timeBetweenThrowsText.text = string.Format("{0:0.##} Seconds", timeThrows);
    }

    // This code may no longer be used, but was to consider if you wanted all the medium level notes, but not the easy ones, or if you do want both.
    public void InclusivityHandler()
    {
        if (inclusivityChoice.value == 0)
        {
            Debug.Log("Inclusive");
            difficulty = "Inclusive";
        }
        if (inclusivityChoice.value == 1)
        {
            difficulty = "Non Inclusive";
        }
    }

    // show the marker or hide it
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

    // Allow baseballs to repeat the target they are thrown to
    public void RepeatHandler()
    {
        if (repeatChoice.value == 0)
        {
            Debug.Log("false");
            repeatMarkers = false;
        }
        if (repeatChoice.value == 1)
        {
            repeatMarkers = true;
        }
    }



    /* Use if using a button approach instead of a dropdown
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

