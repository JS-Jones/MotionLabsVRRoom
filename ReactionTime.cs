using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionTime : MonoBehaviour
{
    // Each target has this code in it.
    public GameObject baseball;
    private BaseballMove BaseballMove;
    public GameObject Speed;
    private ReactionTimeController reactionTimeController;
    public float starttime;
    public float startangle;
    public float endangle;
    private bool marker;

    public GameObject leftGlove;
    public GameObject rightGlove;

    // Whenever the baseball target  is activated, record the time as the start time. and record the angle of the respective glove relative to the center of the user (to be used for angular velocity)
    void OnEnable()
    {
        reactionTimeController = Speed.GetComponent<ReactionTimeController>();
        BaseballMove = baseball.GetComponent<BaseballMove>();
        starttime = Time.time;

        if (transform.name.Equals("Left")) // relative to Left glove
        {
            startangle = BaseballMove.CalculateAngleFromCenter(BaseballMove.center, leftGlove.transform.position);

        } else if (transform.name.Equals("Right")) // relative to right glove
        {
            startangle = BaseballMove.CalculateAngleFromCenter(BaseballMove.center, rightGlove.transform.position);

        }
        marker = false;
    }

    // Whenever the target is hit by the glove, mark the end of the reaction time. 
    private void OnTriggerEnter(Collider other)
    {
  
        if (other.CompareTag("Glove"))
        {
            if (!marker)
            {
                if (BaseballMove != null)
                {
                    // Calculate the angle of the target (which shoudl be same as the angle of the glove when caught)
                    endangle = BaseballMove.CalculateAngleFromCenter(BaseballMove.center, transform.position);

                    // use baseballmove to find endtime and subtract that from the starttime
                    float rt = BaseballMove.RecordReactionTime(starttime);
                    // angular velocity = (endangle - startangle) / rt --> how fast the glove moves to the target
                    float angularVelocity = Mathf.Abs(endangle - startangle) / rt;

                    // Set some limits, below .1 means the target just spawned ontop of the glove
                    if (rt > .1)
                    {
                        reactionTimeController.AddTime(rt, transform.name);
                    }
                    
                    // No shot anyone is moving their hands more than 135 degrees per second.
                    if (angularVelocity < 135)
                    {
                        reactionTimeController.AddAV(angularVelocity, transform.name);
                    }

                    // The above limits looses out on some points, but thats because of how the center point and reaction time are calculated. the ones that work are true data so it works out


                    marker = true;
                    this.enabled = false;
                }
                
            }
            
        }
    }
}
