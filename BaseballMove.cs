using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballMove : MonoBehaviour
{
    // Baseball throw metrics
    private float moveSpeed = 1;
    private float timeBetweenThrows = 5f;
    public string objectToDestroyTag = "Glove";
    private Vector3 initialPosition;
    public bool wasThrown = false;
    private AudioSource audioSource; 

    // Keeps track of tragets
    public List<GameObject> usedTargets = new List<GameObject>();
    public GameObject childGameObject;
    public GameObject target;
    public List<GameObject> parentGameObject;
    public GameObject Area;
    private GameObject nextTarget = null;

    // Reference to calibration code
    public GameObject calibrator;
    public Calibration calibration;
    public Vector3 center;

    // List of all metrics recorder for the report
    public List<float> leftAngles = new List<float>();
    public List<float> missedLeftAngles = new List<float>();
    public List<float> rightAngles = new List<float>();
    public List<float> missedRightAngles = new List<float>();
    public List<Vector2> hitPositions = new List<Vector2>();
    public List<Vector2> missHitPositions = new List<Vector2>();
    public int leftCatches;
    public int leftThrows;
    public int rightCatches;
    public int rightThrows;
    public int totalThrows;

    // Others
    private Material gloveMaterial;
    public GameObject timerObject;
    private Timer timer;
    private float deltaTime;
    

    void OnEnable()
    {
        // record delta time to normalize the time differences always
        deltaTime = Time.deltaTime;

        // recieve and translate ball throwing speed
        string moveSpeedChoice = PlayerPrefs.GetString("ThrowingSpeed", "Slow");
        if (moveSpeedChoice.Equals("Slow"))
        {
            moveSpeed = 1;
        }
        else if (moveSpeedChoice.Equals("Medium"))
        {
            moveSpeed = (float)1.75;
        }
        else if (moveSpeedChoice.Equals("Fast"))
        {
            moveSpeed = (float)2.5;
        }

        // Initialize everything else
        timer = timerObject.GetComponent<Timer>();
        leftCatches = 0;
        rightCatches = 0;
        totalThrows = 0;
        usedTargets.Clear();
        calibration = calibrator.GetComponent<Calibration>();
        center = calibration.centerPoint;

        timeBetweenThrows = PlayerPrefs.GetFloat("TimeThrows", 5f);

        audioSource = GetComponent<AudioSource>(); 

        // calculate the targets that are going to be used
        AvailablePosition();

        // If the gamemode is fixed positions then start the round repeater
        if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Fixed Positions"))
        {

            StartCoroutine(RoundRepeater());
        }
    }

    


    void Update()
    {
        // if the gamemode is freemoving then let the operator control the baseball --> BROKEN
        if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Free Moving"))
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * moveSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right  * moveSpeed);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up  * moveSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down  * moveSpeed);
            }
            if (Input.GetKeyDown(KeyCode.Return) && wasThrown == false)
            {
                initialPosition = transform.position;
                wasThrown = true;
                StartCoroutine(ThrowObject());

            }
        }
    }

    //Moves the baseball at the speed of moveSpeed * throwDirection * deltaTime until z < -2
    IEnumerator ThrowObject()
    {
        Vector3 throwDirection = -Vector3.forward;
        
        while (transform.position.z > -2)
        {
            transform.Translate(moveSpeed * throwDirection * deltaTime);
            if (wasThrown == false)
            {
                
                if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Free Moving")) // Prob wehre free moving issues are
                {
                    transform.position = initialPosition;
                }
                else
                {
                    target.SetActive(false);
                    transform.position = new Vector3(-1, -1, 3);
                } 
                yield break;
            }
            yield return null;
        }

        if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Fixed Positions"))
        {
            target.SetActive(false);
        }
        wasThrown = false;
        yield break; // Exit the coroutine

    }


    // Calculates the available positons
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
            if (PlayerPrefs.GetString("Inclusivity", "Inclusive").Equals("Inclusive"))
            {
                AddChildPositionsToRandomPositions("Easy", randomPositions, leftActive, rightActive);
            }
        }
        else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Hard"))
        {
            AddChildPositionsToRandomPositions("Hard", randomPositions, leftActive, rightActive);
            if (PlayerPrefs.GetString("Inclusivity", "Inclusive").Equals("Inclusive"))
            {
                AddChildPositionsToRandomPositions("Easy", randomPositions, leftActive, rightActive);
                AddChildPositionsToRandomPositions("Medium", randomPositions, leftActive, rightActive);
            }
        }

        return randomPositions;
    }

    // Tied in with the above function
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


    // Whenever the baseball collides with anything.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(objectToDestroyTag))
        {
            //  Play a catching sound
            audioSource.Play();
            wasThrown = false;
            transform.position = new Vector3(-1, -1, 3); 

            // Checks if the round is the actual test and not a warmup
            if (timer.Test)
            {
                //records the postion where it hit
                hitPositions.Add(other.transform.position);
            }
            if (other.transform.parent.gameObject.name.Equals("left_hand"))
            {
                if (timer.Test)
                {
                    // record left hand catch if caught with left hand
                    leftCatches++;
                    leftAngles.Add(CalculateAngleFromCenter(center, other.transform.position));
                }
                

                Transform defaultChild = other.transform.Find("default");

                // changes the material of the glove temporaroily to show caught
                gloveMaterial = defaultChild.GetComponent<Renderer>().material;
                gloveMaterial.color = new Color(0xA7 / 255f, 0xFF / 255f, 0x00 / 255f);

            }
            if (other.transform.parent.gameObject.name.Equals("right_hand"))
            {
                if (timer.Test)
                { 
                    // same as above but for right hand
                    rightCatches++;
                    rightAngles.Add(CalculateAngleFromCenter(center, other.transform.position));
                }
                

                Transform defaultChild = other.transform.Find("default");


                gloveMaterial = defaultChild.GetComponent<Renderer>().material;
                gloveMaterial.color = new Color(0xA7 / 255f, 0xFF / 255f, 0x00 / 255f);
            }

            StartCoroutine(ChangeColorBack());

            // If the ball hits the brick walls in the background then...
        } else if (other.gameObject.CompareTag("BallDestroyer"))
        {
            if (timer.Test)
            {
                // Count as miss
                missHitPositions.Add(target.transform.position);
            }
            if (target.name.Equals("Left"))
            {
                if (timer.Test)
                {
                    // Count as left miss if the target was named left
                    missedLeftAngles.Add(CalculateAngleFromCenter(center, target.transform.position));
                }
                
            }
            if (target.name.Equals("Right"))
            {
                if (timer.Test)
                {
                    // Count as right miss if the target was named right

                    missedRightAngles.Add(CalculateAngleFromCenter(center, target.transform.position));
                }
                
            }
            wasThrown = false;
        }

    }

    // changes the glove color back after 1 second
    private IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(1f);
        if (gloveMaterial != null)
        {
            gloveMaterial.color = Color.white;
        }
    }

    // Core round handler
    IEnumerator RoundRepeater()
    {
        List<Transform> AreaList = AvailablePosition();
        while (true)
        {
            // Move the block to a random position
            Transform randomPosition = MoveBlockToRandom(AreaList);
            

            // Move the block to the chosen position, keeping its z coordinate unchanged
            transform.position = new Vector3(randomPosition.position.x, randomPosition.position.y, 3);

            // Wait for the specified interval before moving again
            if (timer.Test)
            {
                // whenver thrown record a count
                totalThrows++;
            }
            
            if (randomPosition.name.Equals("Left"))
            {
                if (timer.Test)
                {
                    // whenver thrown record a count
                    leftThrows++;
                }
                
            } else if (randomPosition.name.Equals("Right"))
            {
                if (timer.Test)
                {
                    // whenver thrown record a count
                    rightThrows++;
                }
                
            }


            wasThrown = true;
            StartCoroutine(ThrowObject());
            yield return new WaitForSeconds(timeBetweenThrows);
        }
    }

    //reaction time here but really being called from reactionTime.cs
    public float RecordReactionTime(float startTime)
    {
        float reactionTime = Time.time - startTime;
        return reactionTime;
    }


    // Code to calculate angel from the center. We don't want negative numbers or above 180 for each hand/side
    public float CalculateAngleFromCenter(Vector3 center, Vector3 target)
    {
        Vector3 direction = target - center;
        float angle = (Mathf.Atan2(direction.y, direction.x)) * Mathf.Rad2Deg;

        // make it so 0 starts at the bottom
        angle += 90;
        // reduce the angle
        angle %= 360;

        //force positive
        angle = (angle + 360) % 360;

        // allign to 180 to 180 range
        if (angle > 180)
        {
            angle -= 360;
        }

        return angle;
    }


    // Considers weighting to choose which target the ball should go to
    public Transform MoveBlockToRandom(List<Transform> randomPositions)
    {
        if (randomPositions.Count == 0)
        {
            Debug.LogError("No child positions found under randomPositionsParent.");
            return null;
        }

        float weighting = PlayerPrefs.GetFloat("Weighting", 0.5f);
        List<Transform> leftPositions = new List<Transform>();
        List<Transform> rightPositions = new List<Transform>();

        // Separate left and right positions
        foreach (Transform pos in randomPositions)
        {
            if (!usedTargets.Contains(pos.gameObject))
            {
                if (pos.name.Equals("Left"))
                {
                    leftPositions.Add(pos);
                }
                else if (pos.name.Equals("Right"))
                {
                    rightPositions.Add(pos);
                }
            }
           
        }

        // if the weighting is 50% on each side, then all the markers shoudl appear once
        if (weighting == 0.5f)
        {
            if (nextTarget == null || nextTarget.activeSelf)
            {
                int randomIndexA = Random.Range(0, randomPositions.Count);
                Transform randomPositionA = randomPositions[randomIndexA];

                if (timer.Test)
                {
                    if (PlayerPrefs.GetInt("repeatMarkers", 0) == 0)
                    {
                        
                        if (usedTargets.Count >= randomPositions.Count)
                        {
                            usedTargets.Clear();
                            
                        }
                        while (usedTargets.Contains(randomPositionA.gameObject))
                        {
                            randomIndexA = Random.Range(0, randomPositions.Count);
                            randomPositionA = randomPositions[randomIndexA];
                        }
                    }
                }
                
                target = randomPositionA.gameObject;
                if (timer.Test)
                {
                    usedTargets.Add(target);
                }

                // IF showmarkers is true, then the tagets will show before the baseball is thrown
                if (PlayerPrefs.GetInt("ShowMarkers", 1) == 1)
                {
                    target.SetActive(true);
                }
                return randomPositionA;

            // Ignore the else here because nextTarget will always be null, left it here incase you want the system to follow an ordered sequence of throws (left thrown, the right pair of that left is the next thrown)
            } else
            {
                if (PlayerPrefs.GetInt("ShowMarkers", 1) == 1)
                {
                    nextTarget.SetActive(true);
                    target = nextTarget;
                    if (timer.Test)
                    {
                        usedTargets.Add(target);
                    }
     
                    nextTarget = null;
                }
                return target.transform;
            }
          
        }
        else
        {
            // Weight preference
            // Normalize the weights to fit the left and right criteria. SO basically multiply by the follow to find the respective wiehgts:
            float leftWeight = weighting < 0.5f ? (1f - 2 * weighting) : 0;
            float rightWeight = weighting > 0.5f ? 2 * (weighting - 0.5f) : 0;

            // Ensure weights sum to 1
            float totalWeight = leftWeight + rightWeight;

            // Generate a single random value
            float randomValue = Random.Range(0f,1);
            

            // Select a position based on the weighted random value
            Transform selectedPosition = null;
            int randomIndexC;

            // Using a random value compared to the weights to choose whether the ball is thrown 
            if (randomValue < leftWeight && leftPositions.Count > 0)
            {
                // Select a random left position
                randomIndexC = Random.Range(0, leftPositions.Count);
                selectedPosition = leftPositions[randomIndexC];

                if (timer.Test)
                {
                    if (usedTargets.Count >= leftPositions.Count)
                    {
                        usedTargets.Clear();
                    }
                    if (PlayerPrefs.GetInt("repeatMarkers", 0) == 0)
                    {
                        while (usedTargets.Contains(selectedPosition.gameObject))
                        {
                            randomIndexC = Random.Range(0, leftPositions.Count);
                            selectedPosition = leftPositions[randomIndexC];
                        }
                    }
                }

            }
            else if (rightPositions.Count > 0)
            {
                // Select a random right position
                randomIndexC = Random.Range(0, rightPositions.Count);
                selectedPosition = rightPositions[randomIndexC];

                if (timer.Test)
                {
                    if (usedTargets.Count >= rightPositions.Count)
                    {
                        usedTargets.Clear();
                    }
                    if (PlayerPrefs.GetInt("repeatMarkers", 0) == 0)
                    {
                        while (usedTargets.Contains(selectedPosition.gameObject))
                        {
                            randomIndexC = Random.Range(0, rightPositions.Count);
                            selectedPosition = rightPositions[randomIndexC];
                        }
                    }
                }

            }
            else if (leftPositions.Count > 0)
            {
                // Fall back to left positions if no right positions are available
                randomIndexC = Random.Range(0, leftPositions.Count);
                selectedPosition = leftPositions[randomIndexC];
                
            }

            if (selectedPosition != null)
            {
                if (PlayerPrefs.GetInt("ShowMarkers", 1) == 1)
                {
                    target = selectedPosition.gameObject;
                    target.SetActive(true);
                    if (timer.Test)
                    {
                        usedTargets.Add(target);
                    }
                }
                return target.transform;
            }

            return null;

        }

      
    }

}