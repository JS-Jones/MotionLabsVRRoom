using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentNoise : MonoBehaviour
{
    private AudioSource audioSource; 


    void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        if (PlayerPrefs.GetInt("SoundActive", 1) == 1)
        {
            audioSource.Play();
        }
    }

}