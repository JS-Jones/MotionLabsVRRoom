using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calibration : MonoBehaviour
{

    public TMP_Text textMeshPro;
    public string[] textValues;
    private int currentIndex = 0;

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject cam;
    public GameObject baseball;

    Vector3 leftHandPosition;
    Vector3 rightHandPostion;

    public float height;
    public float radius;

    public float maxHeightLeftHand;
    public float maxHeightRightHand;

    public GameObject nodeprefab;
    public GameObject LNode;
    public GameObject RNode;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ChangeTextAfterDelay(.1f));
        }
    }


    IEnumerator ChangeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textMeshPro.text = textValues[currentIndex];
        // -1 = "Calibrating, Press Enter to Continue"
        // 0 = "reach out your hands as straight as possible"
        // 1 = "confirm nodes" --> Intializes height and radius
        // 2 = "stretch out your hands as high as possible" --> Removes nodes from previous method
        // 3 = "confrim nodes" --> Intializes maxHeightLeftHand and Right hand 
        // 4 = "confirm calculated targets" --> removes nodes from previous method and displays all potential targets color coded based on difficulty
        // 5 = "Click Enter to begin" --> removes Displayed targets
        // 6 --> begin game
        if (currentIndex == 1)
        {
            GetHandPositionsOne();
        } else if (currentIndex == 2)
        {
            RemoveNodes();
        } else if (currentIndex == 3)
        {
            GetHandPositionsTwo();
        } else if (currentIndex == 4)
        {
            RemoveNodes();
            DisplayTargets();
        } else if (currentIndex == 5)
        {
            UnDisplayTargets();
        }

        currentIndex++;

        if (currentIndex == 6)
        {
            baseball.SetActive(true);
            gameObject.SetActive(false);
        }
    }


    void GetHandPositionsOne()
    {
        leftHandPosition = leftHand.transform.position;
        rightHandPostion = rightHand.transform.position;

        height = (leftHandPosition.y + rightHandPostion.y) / 2;

        cam.transform.position = new Vector3(cam.transform.position.x, height + 0.15f, cam.transform.position.z);

        radius = (Mathf.Abs(leftHandPosition.x) + Mathf.Abs(rightHandPostion.x)) / 2;

        LNode = Instantiate(nodeprefab, leftHandPosition, Quaternion.identity);
        LNode.GetComponent<AutoDeactivator>().enabled = false;
        RNode = Instantiate(nodeprefab, rightHandPostion, Quaternion.identity);
        RNode.GetComponent<AutoDeactivator>().enabled = false;
    }

    void RemoveNodes()
    {
        Destroy(LNode);
        Destroy(RNode);
    }

    void GetHandPositionsTwo()
    {
        leftHandPosition = leftHand.transform.position;
        rightHandPostion = rightHand.transform.position;

        maxHeightLeftHand = leftHandPosition.y;
        maxHeightRightHand = rightHandPostion.y;

        LNode = Instantiate(nodeprefab, leftHandPosition, Quaternion.identity);
        LNode.GetComponent<AutoDeactivator>().enabled = false;
        RNode = Instantiate(nodeprefab, rightHandPostion, Quaternion.identity);
        RNode.GetComponent<AutoDeactivator>().enabled = false;
    }

    void DisplayTargets()
    {

    }

    void UnDisplayTargets()
    {

    }
}
