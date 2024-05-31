using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySections : MonoBehaviour
{
    public string objectToDestroyTag = "DestroyingSections";

    private void OnTriggerEnter(Collider other)
    {
        name = transform.name;
        if (other.gameObject.CompareTag(objectToDestroyTag) && name == "Section(Clone)")
        {
            // Destroy this object
            Destroy(gameObject);
        }
    }
}

