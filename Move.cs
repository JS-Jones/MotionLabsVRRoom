using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public TMPro.TextMeshProUGUI SpeedUI;
    public GameObject baseball;
    public float moveSpeed = -5; 
    public float speedIncrement = -0.5f;

    
    void Update()
    {
        if (baseball.activeSelf)
        {
            float currentSpeed = float.Parse(SpeedUI.text);
            moveSpeed = -currentSpeed * 5;
            //if (Input.GetKeyDown(KeyCode.S))
            // {
            //    moveSpeed += speedIncrement;
            //}
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    moveSpeed -= speedIncrement;
            //}

            transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);

        }

    }
}
