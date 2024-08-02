using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour

{

    public Transform objectToScale;

    public Vector3 newScale;

    void Start()

    {

        ChangeScale();

    }

    public void ChangeScale()

    {

        // Get the current scale of the object

        Vector3 originalScale = objectToScale.localScale;

        // Calculate the difference between the new scale and the original scale

        // Vector3 scaleDifference = newScale - originalScale;

        // Apply the scale difference to the object's local scale

        objectToScale.localScale = newScale;

        // Reset the scale in the Transform component to (1, 1, 1)

        // objectToScale.localScale /= objectToScale.localScale.magnitude;

    }

}
