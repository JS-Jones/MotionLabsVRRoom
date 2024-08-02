using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuitGame : MonoBehaviour
{
    public GameObject baseball;
    private BaseballMove baseballCode;
    public GameObject timer;
    private Timer timerCode;
    public GameObject canvas;
    public GameObject speedObject;
    private ReactionTimeController RTCode;
   
    // Start is called before the first frame update
    void Start()
    {
        baseballCode = baseball.GetComponent<BaseballMove>();
        RTCode = speedObject.GetComponent<ReactionTimeController>();
        timerCode = timer.GetComponent<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Restart()
    {
        baseballCode.leftAngles.Clear();
        baseballCode.missedLeftAngles.Clear();
        baseballCode.rightAngles.Clear();
        baseballCode.missedRightAngles.Clear();
        baseballCode.leftCatches = 0;
        baseballCode.rightCatches = 0;
        baseballCode.totalThrows = 0;
        baseballCode.hitPositions.Clear();
        baseballCode.missHitPositions.Clear();
        baseballCode.usedTargets.Clear();

        RTCode.reactionTimesLeft.Clear();
        RTCode.reactionTimesRight.Clear();
        RTCode.angularVelocityLeft.Clear();
        RTCode.angularVelocityRight.Clear();


        canvas.SetActive(true);
        timer.SetActive(true);
        timerCode.RestartTimer(); // Call RestartTimer method to restart the timer

        baseball.SetActive(true);
        
        this.transform.parent.gameObject.SetActive(false);
    }
}
