using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballMove : MonoBehaviour
{
    public float moveSpeed = 5;
    public string objectToDestroyTag = "Glove";
    private Vector3 initialPosition;
    private bool wasThrown = false;

    /*public GameObject parentGameObject;
    //public GameObject[] parentGameObjects;*/
    public GameObject childGameObject;
    public GameObject target;

    public List<GameObject> parentGameObject;
    public GameObject rightArea;
    public GameObject leftArea;


    void Start()
    {
        if (PlayerPrefs.GetInt("LeftActive", 1) == 1)
        {
            parentGameObject.Add(leftArea);
        }
        if (PlayerPrefs.GetInt("Right Active", 1) == 1)
        {
            parentGameObject.Add(rightArea);
        }
        if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Fixed Positions")) 
        {
            
            //new WaitForSeconds(7);
            StartCoroutine(RoundRepeater());
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetString("Gamemode", "Fixed Positions").Equals("Free Moving")) 
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
            }
            if (Input.GetKeyDown(KeyCode.Return) && wasThrown == false)
            {
                initialPosition = transform.position;
                // print("IP: " + initialPosition);
                wasThrown = true;
                StartCoroutine(ThrowObject());

            }
        }
    }

    IEnumerator ThrowObject()
    {
        Vector3 throwDirection = -Vector3.forward;
        while (transform.position.z > -2)
        {
            transform.Translate(moveSpeed * Time.deltaTime * throwDirection);
            if (wasThrown == false)
            {
                target.SetActive(false);
                transform.position = new Vector3(-1, -1, 3);
                yield break;
            }
            yield return null;
        }
        //transform.position = new Vector3(-1, -1, 3);
        target.SetActive(false);
        wasThrown = false;
        yield break; // Exit the coroutine

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(objectToDestroyTag) || transform.position.z <= -2)
        {
            // transform.position = initialPosition;
            wasThrown = false;
        }
    }

    IEnumerator RoundRepeater()
    {
        List<Transform> AreaList = AvailablePosition();
        while (true)
        {
            // Move the block to a random position
            Transform randomPosition = MoveBlockToRandom(AreaList);

            // Move the block to the chosen position, keeping its z coordinate unchanged
            transform.position = new Vector3(randomPosition.position.x, randomPosition.position.y, 3);

            //transform.position = initialPosition;
            // Wait for the specified interval before moving again


            wasThrown = true;
            StartCoroutine(ThrowObject());
            yield return new WaitForSeconds(5);
        }
    }

    public List<Transform> AvailablePosition()
    {
        List<Transform> randomPositions = new List<Transform>();
        for (int y = 0; y <parentGameObject.Count; y++)
        {
            if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Easy"))
            {
                Transform difficulty = parentGameObject[y].transform.Find("Easy");
                if (difficulty != null)
                {
                    childGameObject = difficulty.gameObject;
                }

                for (int i = 0; i < childGameObject.transform.childCount; i++)
                {
                    randomPositions.Add(childGameObject.transform.GetChild(i));
                }
      
            }


            else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Medium"))
            {
                Transform difficulty = parentGameObject[y].transform.Find("Medium");
                if (difficulty != null)
                {
                    childGameObject = difficulty.gameObject;
                }

                if (PlayerPrefs.GetInt("Inclusive", 1) == 1)
                {
                    for (int i =0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                    childGameObject = parentGameObject[y].transform.Find("Easy").gameObject;
                    for (int i = 0; i< childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                } 
                else
                {
                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                }
            }
            else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Hard"))
            {
                Transform difficulty = parentGameObject[y].transform.Find("Hard");
                if (difficulty != null)
                {
                    childGameObject = difficulty.gameObject;
                }

                if (PlayerPrefs.GetInt("Inclusive", 1) == 1)
                {
                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                    childGameObject = parentGameObject[y].transform.Find("Easy").gameObject;
                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                    childGameObject = parentGameObject[y].transform.Find("Medium").gameObject;
                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                }
                else
                {
                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }
                }
            }
        }
        return randomPositions;
    }

    public Transform MoveBlockToRandom(List<Transform> randomPositions)
    {
        if (randomPositions.Count == 0)
        {
            Debug.LogError("No child positons found under randomPositonsParent");
        }

        int randomIndex = Random.Range(0, randomPositions.Count);
        Transform randomPosition = randomPositions[randomIndex];
        target = randomPosition.gameObject;
        target.SetActive(true);

        return (randomPosition);
    }

        /*
            public Transform MoveBlockToRandom()
            {
                List<Transform> randomPositions = new();


                //PlayerPrefs.SetString("Difficulty", "Easy"); // REMOVE THIS
                // Get all the children of the randomPositionsParent
                if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Easy"))
                {
                    Transform difficulty = parentGameObject.transform.Find("Easy");

                    if (difficulty != null)
                    {
                        childGameObject = difficulty.gameObject;

                    }

                    for (int i = 0; i < childGameObject.transform.childCount; i++)
                    {
                        randomPositions.Add(childGameObject.transform.GetChild(i));
                    }

                    //childTransform = difficulty;
                }
                else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Medium"))
                {
                    Transform difficulty = parentGameObject.transform.Find("Medium");

                    if (difficulty != null)
                    {
                        childGameObject = difficulty.gameObject;

                    }



                    if (PlayerPrefs.GetInt("Inclusive", 1) == 1) // Add this option
                    {
                        // Get all the children from each randomPositionsParent
                        for (int i = 0; i < childGameObject.transform.childCount; i++)
                        {
                            randomPositions.Add(childGameObject.transform.GetChild(i));
                        }

                        childGameObject = parentGameObject.transform.Find("Easy").gameObject;
                        for (int i = 0; i < childGameObject.transform.childCount; i++)
                        {
                            Debug.Log("truewwwww");

                            randomPositions.Add(childGameObject.transform.GetChild(i));
                        }
                    }

                }
                else if (PlayerPrefs.GetString("Difficulty", "Easy").Equals("Hard"))
                {
                    Transform difficulty = parentGameObject.transform.Find("Hard");

                    if (difficulty != null)
                    {
                        childGameObject = difficulty.gameObject;

                    }

                    if (PlayerPrefs.GetInt("Inclusive", 1) == 1) // Add this option
                    {
                        // Get all the children from each randomPositionsParent
                        for (int i = 0; i < childGameObject.transform.childCount; i++)
                        {
                            randomPositions.Add(childGameObject.transform.GetChild(i));
                        }

                        childGameObject = parentGameObject.transform.Find("Easy").gameObject;
                        for (int i = 0; i < childGameObject.transform.childCount; i++)
                        {
                            //Debug.Log("truewwwww");

                            randomPositions.Add(childGameObject.transform.GetChild(i));
                        }
                        childGameObject = parentGameObject.transform.Find("Medium").gameObject;
                        for (int i = 0; i < childGameObject.transform.childCount; i++)
                        {
                            //Debug.Log("truewwwww");

                            randomPositions.Add(childGameObject.transform.GetChild(i));
                        }
                    }

                }

                //int childCount = childGameObject.transform.childCount;

                if (randomPositions.Count == 0)
                {
                    Debug.LogError("No child positions found under randomPositionsParent.");
                }

                int randomIndex = Random.Range(0, randomPositions.Count);
                Transform randomPosition = randomPositions[randomIndex];
                target = randomPosition.gameObject;
                target.SetActive(true);

                // Choose a random index
                //int randomIndex = Random.Range(0, childCount);

                // Get the child transform at the random index
                // Transform randomPosition = childGameObject.transform.GetChild(randomIndex);
                ////Debug.Log(randomPosition.position.x);
                // //Debug.Log(randomPosition.position.y);

                return (randomPosition);

            }*/
    }
