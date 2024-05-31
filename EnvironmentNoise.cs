using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentNoise : MonoBehaviour
{
    private AudioSource audioSource; // ADD TO NEW VERSION

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // ADD TO NEW VERSION
        if (PlayerPrefs.GetInt("LeftActive", 1) == 1)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
