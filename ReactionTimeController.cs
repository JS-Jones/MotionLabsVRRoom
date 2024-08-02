using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTimeController : MonoBehaviour
{
    // Stores the following Report items
    public List<float> reactionTimesLeft = new List<float>();
    public List<float> reactionTimesRight = new List<float>();

    public List<float> angularVelocityLeft = new List<float>();
    public List<float> angularVelocityRight = new List<float>();

    public GameObject timerObject;
    private Timer timer;

    private void Start()
    {
        timer = timerObject.GetComponent<Timer>();
    }

    // Basically stores whatever reaction time was passed into this method into a list left or right
    public void AddTime(float timing, string area)
    {
        if (timer.Test)
        {
            if (area.Equals("Left"))
            {
                reactionTimesLeft.Add(timing);
            }
            else if (area.Equals("Right"))
            {
                reactionTimesRight.Add(timing);
            }
        }
        
    }

    // Basically stores whatever angular velocity was passed into this method into a list left or right
    public void AddAV(float angularVelocity, string area)
    {
        if (timer.Test)
        {
            if (area.Equals("Left"))
            {
                angularVelocityLeft.Add(angularVelocity);
            }
            else if (area.Equals("Right"))
            {
                angularVelocityRight.Add(angularVelocity);
            }
        }
        
    }
}
