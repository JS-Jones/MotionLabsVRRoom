using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public int duration;
    public int timeRemaining;
    public bool isCountingDown = false;
    public TMP_Text timerText;
    public TMP_Text timerTitle;
    public TMP_Text message;
    private FlashingColor flashing;
    
    public Image clock;
    public GameObject baseball;
    public GameObject report;
    public GameObject canvas;

    public string clockType;
    private Coroutine countdownCoroutine;

    public GameObject treadmill;
    private PythonTrigger pythonTrigger;
    public GameObject lefthand;
    public GameObject rightHand;

    public bool Test; // global check in all cases to record metrics, not during the buffer period

    // This code determines what the desired duration is and what time to clock to show.
    public void OnEnable()
    {
        pythonTrigger = treadmill.GetComponent<PythonTrigger>();
        flashing = message.gameObject.GetComponent<FlashingColor>();

        clockType = PlayerPrefs.GetString("TimerType", "Radial");
        duration = PlayerPrefs.GetInt("Duration", 120);
        timeRemaining = duration;

        SetupClock();

        if (!isCountingDown)
        {
            StartCountdown();
        }
    }

    // Adjust the clock according to the style
    private void SetupClock()
    {
        timerText.text = duration.ToString();

        if (clockType.Equals("Timed"))
        {
            timerText.gameObject.SetActive(true);
            timerTitle.gameObject.SetActive(true);
            clock.gameObject.SetActive(false);
        }
        else if (clockType.Equals("Radial"))
        {
            timerText.gameObject.SetActive(false);
            timerTitle.gameObject.SetActive(false);
            clock.gameObject.SetActive(true);
            clock.fillAmount = 1f; // Ensure the clock is full at the start
        }
        else if (clockType.Equals("None"))
        {
            canvas.SetActive(false);
        }
    }

    // Starting an actual round now
    private IEnumerator Tick()
    {
        
        // Begin with a 20 second warmup session
        timerText.text = "20";
        timeRemaining = 20;
        Test = false;
        message.text = "Warm-up Round";
        
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);

            timeRemaining--;
            if (clockType.Equals("Timed"))
            {
                timerText.text = timeRemaining.ToString();
            }
            else if (clockType.Equals("Radial"))
            {
                clock.fillAmount = (float)timeRemaining / 20;
            }
            if (timeRemaining <= 5)
            {
                // at 5 seconds, start flasshing the main text to warn the user.
                flashing.enabled = true;
            }
        }
        // change the text color to white to rest and stop flashing
        flashing.enabled = false;     
        message.color = Color.white;

        // Now starting the actual test
        timerText.text = duration.ToString();
        timeRemaining = duration;
        Test = true;
        message.text = "Test Round";
        while (timeRemaining > 0)
        {
            flashing.enabled = false;
            message.color = Color.white;
            yield return new WaitForSeconds(1f);

            timeRemaining--;
            if (clockType.Equals("Timed"))
            {
                timerText.text = timeRemaining.ToString();
            }
            else if (clockType.Equals("Radial"))
            {
                clock.fillAmount = (float)timeRemaining / duration;
            }
            
        }
        
        // After the countdown is finished, deactivate baeball, canvas, and activate the report + stop treadmill.
        message.color = Color.white;
        isCountingDown = false;
        baseball.SetActive(false);
        canvas.SetActive(false);
        report.SetActive(true);
        pythonTrigger.Stopper();
        //SceneManager.LoadScene(0);
        

    }

    /*private void Update()
    {
        if ((lefthand.transform.position.z + rightHand.transform.position.z) / 2 < .6)
        {
            StopCountdown();
            StartCoroutine(DisplayMessage("Round Paused: Please Move Up"));
            StartCountdown();
        }
    }*/

    // Easy way to make the countdown tick
    public void StartCountdown()
    {
        isCountingDown = true;
        countdownCoroutine = StartCoroutine(Tick());
    }

    // Has now use at the moment, but this is meant to just flash the text and warn them of something. FOr example, moving back too much
    private IEnumerator DisplayMessage(string note)
    {
        message.text = note;
        flashing.enabled = true;
        yield return new WaitForSeconds(2);
        flashing.enabled = false;
        message.color = Color.white;
    }

    // Stop the countdown if pausing or something
    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        isCountingDown = false;
    }

    // Easy way to restart timer, especially after the end of a round. Called by the Report code.
    public void RestartTimer()
    {
        StopCountdown();
        timeRemaining = duration;
        SetupClock();
        StartCountdown();
    }
}
