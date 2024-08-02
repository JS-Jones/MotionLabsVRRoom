using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private GameObject otherObject;
    private string symmetry;

    void Awake()
    {
        // Automatically find and assign the other object based on the name
        if (gameObject.name == "Right")
        {
            otherObject = transform.parent.Find("Left").gameObject;
        }
        else if (gameObject.name == "Left")
        {
            otherObject = transform.parent.Find("Right").gameObject;
        }

        if (otherObject == null)
        {
            otherObject = null;
            //Debug.LogError("Other object not found. Make sure the names are 'Left' and 'Right'.");
        }
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        // Checks to move partner node also or not
        symmetry = PlayerPrefs.GetString("Symmetry", "true");

    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        Vector3 originalPosition = transform.position;
        Vector3 newPosition = GetMouseAsWorldPoint() + mOffset;
        transform.position = newPosition;

        if (otherObject != null)
        {
            // If mvoing the other node also then ...
            if (symmetry.Equals("true"))
            {
                Vector3 currentPosition = otherObject.transform.position;
                // Calculate the mirrored position for the other object
                Vector3 mirroredPosition = new Vector3(currentPosition.x + (originalPosition.x - newPosition.x), currentPosition.y - (originalPosition.y - newPosition.y), newPosition.z);

                otherObject.transform.position = mirroredPosition;
            }
        }
    }
}
