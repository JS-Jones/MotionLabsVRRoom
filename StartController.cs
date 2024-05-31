using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    public GameObject LeftBaseballGlove; 
    public GameObject RightBaseballGlove; 

    void Start()
    {
        bool leftActive = PlayerPrefs.GetInt("LeftActive") == 1;
        bool rightActive = PlayerPrefs.GetInt("RightActive") == 1;

        if (!leftActive)
        {
            LeftBaseballGlove.SetActive(false);
        }
        if (!rightActive)
        {
            RightBaseballGlove.SetActive(false);
        }
        
        Debug.Log("Left Active: " + leftActive);
        Debug.Log("Right Active: " + rightActive);
    }
}
