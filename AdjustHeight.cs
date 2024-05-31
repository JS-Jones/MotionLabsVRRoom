using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustHeight : MonoBehaviour
{
    // Speed of upward and downward movement
    public float moveSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.Q)) {
        transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
        }
    }
}
