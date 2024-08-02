using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public string[] textValues;
    private int currentIndex = 0;

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject cam;
    public GameObject baseball;
    public GameObject treadmill;
    private PythonTrigger pythonTrigger;
    public GameObject timer;
    public GameObject canvas;

    Vector3 leftHandPosition;
    Vector3 rightHandPostion;

    public float height;
    public float radius;
    public Vector3 centerPoint;

    public float maxHeightLeftHand;
    public float maxHeightRightHand;

    public GameObject nodeprefab;
    public GameObject LNode;
    public GameObject RNode;

    public List<GameObject> parentGameObject;

    public GameObject Area;

    public GameObject childGameObject;
    public List<Transform> AreaList;

    public GameObject intensity;
    public GameObject sections;

    public TMP_Dropdown Symmertry;

    void Start()
    {
        // Add the Area object to the parentGameObject list
        parentGameObject.Add(Area);
        pythonTrigger = treadmill.GetComponent<PythonTrigger>();

    }

    // Allows the user to proceed through the different phases
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(ChangeTextAfterDelay(.1f));
        }
    }

    // Function to change whether the target nodes move together when dragged or seperately
    public void SymmetryTypeChanged()
    {
        if (Symmertry.value == 0)
        {
            Debug.Log(1);
            PlayerPrefs.SetString("Symmetry", "true");
        }
        if (Symmertry.value == 1)
        {
            Debug.Log(2);
            PlayerPrefs.SetString("Symmetry", "false");
        }
    }

    //Changes the bottom info text as the player proceeds through the calibration
    IEnumerator ChangeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textMeshPro.text = textValues[currentIndex];
        // -1 = "Calibrating, Press Enter to Continue"
        // 0 = "reach out your hands as straight as possible"
        // 1 = "confirm nodes" --> Intializes height and radius
        // 2 = "stretch out your hands as high as possible" --> Removes nodes from previous method
        // 3 = "confirm nodes" --> Intializes maxHeightLeftHand and Right hand 
        // 4 = "confirm calculated targets" --> removes nodes from previous method and displays all potential targets color coded based on difficulty
        // 5 = "Click Enter to begin" --> removes Displayed targets
        // 6 --> begin game
        if (currentIndex == 1)
        {
            GetHandPositionsOne();
        }
        else if (currentIndex == 2)
        {
            RemoveNodes();
        }
        else if (currentIndex == 3)
        {
            GetHandPositionsTwo();
        }
        else if (currentIndex == 4)
        {
            RemoveNodes();
            DisplayTargets();
            CalibrateDisplayTargets();
        }
        else if (currentIndex == 5)
        {
            UnDisplayTargets();
            sections.SetActive(true);
        }

        currentIndex++;

        // starts the game after reaching this index
        if (currentIndex == 7)
        {
            baseball.SetActive(true);
            treadmill.SetActive(true);
            pythonTrigger.Starter();
            timer.SetActive(true);
            canvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void GetHandPositionsOne()
    {
        // Get the position of the hands after stretching otu adn calucluate center point
        leftHandPosition = leftHand.transform.position;
        rightHandPostion = rightHand.transform.position;

        centerPoint = (leftHand.transform.position + rightHand.transform.position) / 2;

        height = (leftHandPosition.y + rightHandPostion.y) / 2;

        // Change the camera based on user height
        cam.transform.position = new Vector3(cam.transform.position.x, height + 0.35f, cam.transform.position.z);

        radius = (Mathf.Abs(leftHandPosition.x) + Mathf.Abs(rightHandPostion.x)) / 2;

        // create nodes to confirm simulation --> May want to move all of the above to another step after the instantiate in case the operator wants to adjust the nodes that appear
        LNode = Instantiate(nodeprefab, leftHandPosition, Quaternion.identity);
        RNode = Instantiate(nodeprefab, rightHandPostion, Quaternion.identity);
    }

    void RemoveNodes()
    {
        // removes the confirmation nodes
        Destroy(LNode);
        Destroy(RNode);
    }

    void GetHandPositionsTwo()
    {
        // when the player stretches their arms all the way up --> these values are not really used in any further calculation, may want to remove this step.
        leftHandPosition = leftHand.transform.position;
        rightHandPostion = rightHand.transform.position;

        maxHeightLeftHand = leftHandPosition.y;
        maxHeightRightHand = rightHandPostion.y;

        LNode = Instantiate(nodeprefab, leftHandPosition, Quaternion.identity);
        RNode = Instantiate(nodeprefab, rightHandPostion, Quaternion.identity);
    }

    void DisplayTargets()
    {
        // Displays all potential targets based on the options menu
        intensity.SetActive(true);
        Symmertry.gameObject.transform.parent.gameObject.SetActive(true);

        //Scles the parent object of the nodes so that it better fits the range of motion for all people
        //A range of motion intensity circle is show for easy, medium, and hard to help the therapist better adjust the nodes to their liking.
        float scaling = (float)((radius - .7));
        intensity.transform.localScale = new Vector3((float)(1 + scaling * 2.5), 1 + scaling, scaling);

        //Calculate potential positions
        AreaList = AvailablePosition();

        //Disable the AutoDeactivator and ReactionTime scripts in the poential positons - we dont need it running right now
        foreach (Transform area in AreaList)
        {
            if (area != null) 
            {
                area.gameObject.GetComponent<AutoDeactivator>().enabled = false;
                area.gameObject.GetComponent<ReactionTime>().enabled = false;
                area.gameObject.SetActive(true);
            }
        }
    }

    // CHeck the readme to undertsnad how the nodes are organized
    public List<Transform> AvailablePosition()
    {
        List<Transform> randomPositions = new List<Transform>();
        bool leftActive = PlayerPrefs.GetInt("LeftActive", 1) == 1;
        bool rightActive = PlayerPrefs.GetInt("RightActive", 1) == 1;

        if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Easy"))
        {
            AddChildPositionsToRandomPositions("Easy", randomPositions, leftActive, rightActive);
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Medium"))
        {
            AddChildPositionsToRandomPositions("Medium", randomPositions, leftActive, rightActive);
            if (PlayerPrefs.GetString("Inclusivity", "Inclusive").Equals("Inclusive")) // ignore
            {
                AddChildPositionsToRandomPositions("Easy", randomPositions, leftActive, rightActive);
            }
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Hard"))
        {
            AddChildPositionsToRandomPositions("Hard", randomPositions, leftActive, rightActive);
            if (PlayerPrefs.GetString("Inclusivity", "Inclusive").Equals("Inclusive")) // ignore
            {
                AddChildPositionsToRandomPositions("Easy", randomPositions, leftActive, rightActive);
                AddChildPositionsToRandomPositions("Medium", randomPositions, leftActive, rightActive);
            }
        }

        return randomPositions;
    }

    // Interjoing with the above to add it the random postions list based on difficulty level
    private void AddChildPositionsToRandomPositions(string difficultyLevel, List<Transform> randomPositions, bool leftActive, bool rightActive)
    {
        Transform difficulty = Area.transform.Find(difficultyLevel);
        if (difficulty != null)
        {

            for (int i = 0; i < difficulty.childCount; i++)
            {
     
       
                Transform node = difficulty.GetChild(i);
                for (int j = 0; j < node.childCount; j++)
                {
                    Transform nodej = node.GetChild(j);
                    if ((leftActive && nodej.name.Contains("Left")) || (rightActive && nodej.name.Contains("Right")))
                    {
                     
                        randomPositions.Add(nodej);
                    }
                }
            }
        }
    }

    // Calibrates the positon of the targets on the y and z axis
    void CalibrateDisplayTargets()
    {
        foreach (Transform area in AreaList)
        {
            if (area != null) // Check if the Transform is not null
            {
                Vector3 newPosition = area.position;
                newPosition.y = (float)(newPosition.y + (centerPoint.y - 1.3));
                newPosition.z = (float)(centerPoint.z + .15);
                area.position = newPosition;
            }
        }
    }

    // makes the targets transparent and enables eveything we disabled at the start.
    void UnDisplayTargets()
    {
        intensity.SetActive(false);
        Symmertry.gameObject.transform.parent.gameObject.SetActive(false);
        foreach (Transform area in AreaList)
        {
            if (area != null) // Check if the Transform is not null
            {
                area.gameObject.GetComponent<AutoDeactivator>().enabled = true;
                area.gameObject.GetComponent<ReactionTime>().enabled = true;
                area.gameObject.SetActive(false);
            }
        }
    }
}
