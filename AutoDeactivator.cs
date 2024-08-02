using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivator : MonoBehaviour
{
    // This code is a backup to removing the marker after the baseball passes
    private float deactiveTime = 5f;

    //After the marker is activated, set active as false after 5 seconds
    void OnEnable()
    {
        deactiveTime = PlayerPrefs.GetFloat("TimeThrows", 5f);
        transform.gameObject.GetComponent<ReactionTime>().enabled = true;
        StartCoroutine(DeactivateAfterDelay());

    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(deactiveTime);
        gameObject.SetActive(false);
    }
}
