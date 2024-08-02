using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAdjuster : MonoBehaviour
{
    public TMPro.TextMeshProUGUI SpeedUI;

    void Start()
    {
        string textValue = SpeedUI.text;
        
    }

    void Update()
    {
        
    }

    public void AddSpeed()
    {
        double currentSpeed = double.Parse(SpeedUI.text);
        if (currentSpeed >= 3.5f)
        {
            SpeedUI.text = "3.5";
        }
        else
        {
            SpeedUI.text = (currentSpeed + .1).ToString();
        }
        
    }

    public void MinusSpeed()
    {
        double currentSpeed = double.Parse(SpeedUI.text);
        if (currentSpeed <= 0f)
        {
            SpeedUI.text = "0";
        }
        else
        {
            SpeedUI.text = (currentSpeed - .1).ToString();
        }

    }
}
