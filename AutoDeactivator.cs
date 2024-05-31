using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivator : MonoBehaviour
{
    // This code is a backup to removing the marker after the baseball passes

    public float deactiveTime = 7f;
    
    void onEnable()
    {
        StartCoroutine(DeactivateAfterDelay());

    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(deactiveTime);
        gameObject.SetActive(false);
    }
}
