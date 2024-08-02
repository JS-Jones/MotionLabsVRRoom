using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReportCode : MonoBehaviour
{
    // List of all the values being record
    private float accuracy;
    private int leftHandCatches;
    private int leftThrows;
    private int rightHandCatches;
    private int rightThrows;
    private int totalThrows;
    private List<float> reactionTimesLeft;
    private List<float> reactionTimesRight;
    private List<float> rightAngleList;
    private List<float> rightMissAngleList;
    private List<float> leftAngleList;
    private List<float> leftMissAngleList;
    private List<float> leftAngularVelocity;
    private List<float> rightAngularVelocity;
    private string speed;

    public GameObject baseball;
    private BaseballMove baseballScript;
    public GameObject speedcodeobject;
    private ReactionTimeController reactionTimeCode;

    // List of the components to adjust for reporting
    public TMP_Text titleText;
    public TMP_Text accuracyText;
    public TMP_Text leftHandCatchesText;
    public TMP_Text rightHandCatchesText;
    public TMP_Text reactionTimeTextLeft;
    public TMP_Text reactionTimeTextRight;
    public TMP_Text leftAngularVelocityText;
    public TMP_Text rightAngularVelocityText;
    public TMP_Text speedText;
    public TMP_Text averageMissAngleText;
    public TMP_Text maxRightAngleText;
    public TMP_Text maxLeftAngleText;
    public Image ROMRight;
    public Image ROMLeft;
    public Image TemplatePrefab;
    public Image background;
    private string difficulty;
    private int targetCount;
    private List<Image> templateInstances = new List<Image>(); // List to hold instantiated Template instances to clear afterwards

    public RectTransform canvasRectTransform;
    private List<Vector2> hitPositions;
    private List<Vector2> missHitPositions;

    // Start is called before the first frame update
    void Start() // Change this to OnEnable, havent had time to make this change, but important for if the patient does multiple rounds.
    {
        // Set all the Values
        templateInstances.Clear();
        baseballScript = baseball.GetComponent<BaseballMove>();
        reactionTimeCode = speedcodeobject.GetComponent<ReactionTimeController>();
        totalThrows = baseballScript.totalThrows;
        leftHandCatches = baseballScript.leftCatches;
        leftThrows = baseballScript.leftThrows;
        rightThrows = baseballScript.rightThrows;
        rightHandCatches = baseballScript.rightCatches;
        reactionTimesLeft = reactionTimeCode.reactionTimesLeft;
        reactionTimesRight = reactionTimeCode.reactionTimesRight;
        leftAngularVelocity = reactionTimeCode.angularVelocityLeft;
        rightAngularVelocity = reactionTimeCode.angularVelocityRight;
        leftAngleList = baseballScript.leftAngles;
        rightAngleList = baseballScript.rightAngles;
        leftMissAngleList = baseballScript.missedLeftAngles;
        rightMissAngleList = baseballScript.missedRightAngles;
        hitPositions = baseballScript.hitPositions;
        missHitPositions = baseballScript.missHitPositions;
        //hitPositions.Add(new Vector2((float)-0.092, (float).163));
       //hitPositions.Add(new Vector2((float).846, (float)-0.127));
        speed = PlayerPrefs.GetString("StartingSpeed", "0");
        difficulty = PlayerPrefs.GetString("Difficulty", "Easy");

        // Calculate the difficulty into terms of targets
        if (difficulty.Equals("Easy"))
        {
            targetCount = 24;
        } else if (difficulty.Equals("Medium"))
        {
            targetCount = 38;
        } else if (difficulty.Equals("Medium"))
        {
            targetCount = 52;
        }

        // change title text
        titleText.text = targetCount.ToString() + " Count - " + PlayerPrefs.GetInt("Duration", 120).ToString() + " Seconds - " + PlayerPrefs.GetString("ThrowingSpeed", "Slow") + " Throws";

        // Emsures that catches doesnt exceed throws
        if ((leftHandCatches) > leftThrows)
        {
            leftHandCatches = leftThrows;
        }
        if ((rightHandCatches) > rightThrows)
        {
            rightHandCatches = rightThrows;
        }
        //Calculate accuracy
        accuracy = (leftHandCatches + rightHandCatches) / (float)totalThrows;
        accuracyText.text = (accuracy * 100f).ToString("F2") + "% \n(" + (leftHandCatches + rightHandCatches) + "/ " + totalThrows + " throws)";



        float leftAccuracy = (leftHandCatches) / (float)leftThrows;
        leftHandCatchesText.text = (leftAccuracy * 100f).ToString("F2") + "% \n(" + (leftHandCatches) + "/ " + leftThrows + " throws)";
        float rightAccuracy = (rightHandCatches) / (float)rightThrows;
        rightHandCatchesText.text = (rightAccuracy * 100f).ToString("F2") + "% \n(" + (rightHandCatches) + "/ " + rightThrows + " throws)";

        // Calculate average reaction time and display
        float averageReactionTimeLeft = CalculateAverage(reactionTimesLeft);
        reactionTimeTextLeft.text = averageReactionTimeLeft.ToString("F2") + " seconds";

        float averageReactionTimeRight = CalculateAverage(reactionTimesRight);
        reactionTimeTextRight.text = averageReactionTimeRight.ToString("F2") + " seconds";

        float averageAngularVelocityRight = CalculateAverage(rightAngularVelocity);
        rightAngularVelocityText.text = averageAngularVelocityRight.ToString("F2") + " °/sec";

        float averageAngularVelocityLeft = CalculateAverage(leftAngularVelocity);
        leftAngularVelocityText.text = averageAngularVelocityLeft.ToString("F2") + " °/sec";



        speedText.text = speed;
        // average missed acngles

        List<float> missedcombinedList = new List<float>();
        if (leftMissAngleList != null)
        {
            for (int i = 0; i < leftMissAngleList.Count; i++)
            {
                leftMissAngleList[i] = Mathf.Abs(leftMissAngleList[i]);
            }
            missedcombinedList.AddRange(leftMissAngleList);
        }
        if (rightMissAngleList != null)
        {
            for (int i = 0; i < rightMissAngleList.Count; i++)
            {
                rightMissAngleList[i] = Mathf.Abs(rightMissAngleList[i]);
            }
            missedcombinedList.AddRange(rightMissAngleList);
        }

        float averageMissAngle = CalculateAverage(missedcombinedList);
        averageMissAngleText.text = averageMissAngle.ToString("F2") + "°";

        for (int i = 0; i < leftAngleList.Count; i++)
        {
            leftAngleList[i] = Mathf.Abs(leftAngleList[i]);
        }
        for (int i = 0; i < rightAngleList.Count; i++)
        {
            rightAngleList[i] = Mathf.Abs(rightAngleList[i]);
        }

        // Max left and right caught angle
        float maxLeft = (Mathf.Max(leftAngleList.ToArray()));
        float maxRight = (Mathf.Max(rightAngleList.ToArray()));

        if (maxLeft > 180)
        {
            maxLeft = 180;
        }
        if (maxRight > 180)
        {
            maxRight = 180;
        }

        maxLeftAngleText.text = "Left: " + maxLeft + "°";
        maxRightAngleText.text = "Right: " + maxRight + "°";

        ROMRight.fillAmount = maxRight / 360;
        ROMLeft.fillAmount = maxLeft / 360;

        // Align Template image based on angles
        InstantiateTemplates();
        // allight the template image based on position on screen
        ShowHitReport();
    }

    // determines the world to canvas ratio and then uses that to plot where the targets where relative to the person
    void ShowHitReport()
    {
        foreach (Vector2 hitPosition in hitPositions)
        {
            Image hitMarker = Instantiate(TemplatePrefab, canvasRectTransform);

            Vector2 canvasPos = WorldtoCanvasPosition(hitPosition);

            hitMarker.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            templateInstances.Add(hitMarker);
        }
        foreach (Vector2 missHitPosition in missHitPositions)
        {
            Image hitMarker = Instantiate(TemplatePrefab, canvasRectTransform);

            Vector2 canvasPos = WorldtoCanvasPosition(missHitPosition);

            hitMarker.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            hitMarker.color = Color.red;
            templateInstances.Add(hitMarker);
        }
    }
    // Uses screentoviewportposition to determine position. Multiples by a portion to make the distance changes larger
    Vector2 WorldtoCanvasPosition(Vector2 worldPosition)
    {
        Vector2 viewportPosition = Camera.main.ScreenToViewportPoint(worldPosition);
        Vector2 canvasSize = canvasRectTransform.sizeDelta * 250;
        return new Vector2(-viewportPosition.x * canvasSize.x, -viewportPosition.y * canvasSize.y);
    }

    // Method to calculate average reaction time
    private float CalculateAverage(List<float> targetList)
    {
        float sum = 0f;

        // Ensure targetList list is not null and has elements
        if (targetList != null && targetList.Count > 0)
        {
            // Sum all targetList in the list
            foreach (float time in targetList)
            {
                sum += Mathf.Abs(time);
            }

            // Calculate average
            return sum / targetList.Count;
        }
        else
        {
            // If targetList list is null or empty, return 0 (or handle appropriately for your case)
            return 0f;
        }
    }

    // Method to align Template image based on angles and background circle
    private void InstantiateTemplates()
    {
        if (TemplatePrefab == null)
        {
            Debug.LogWarning("Template prefab not assigned in the Inspector.");
            return;
        }

        if (background == null)
        {
            Debug.LogWarning("Background image not assigned in the Inspector.");
            return;
        }

        // Get background circle's center position
        Vector3 centerPosition = background.rectTransform.position;

        // Assuming background circle's size represents its diameter
        float radius = background.rectTransform.sizeDelta.x / 2f;


        // Instantiate Template images based on leftAngleList and rightAngleList
        for (int i = 0; i < leftAngleList.Count; i++)
        {
            
            float leftAngle = leftAngleList[i];

            // Calculate position on circumference based on angle
            float radians = (float)(Mathf.Abs(Mathf.PI- (leftAngle * Mathf.Deg2Rad - Mathf.PI/2)));
            float x = (float)(centerPosition.x + radius * Mathf.Cos(radians) * 3.5); // 3.75
            float y = (float)(centerPosition.y + radius * Mathf.Sin(radians) * 3.5);

            // Instantiate Template prefab and set its position
            Image templateInstance = Instantiate(TemplatePrefab, transform); // Instantiate under ReportCode GameObject
            //RectTransform templateRectTransform = templateInstance.rectTransform;
            //templateRectTransform.anchoredPosition = new Vector2(x, y);
            templateInstance.transform.position = new Vector2(x, y);
            // Add instantiated Template to list
            templateInstances.Add(templateInstance);
        }

        for (int i = 0; i < rightAngleList.Count; i++)
        {
            
            float rightAngle = rightAngleList[i];

            // Calculate position on circumference based on angle
            float radians = (float)(rightAngle * Mathf.Deg2Rad + (3*Mathf.PI/2));
            float x = (float)(centerPosition.x + radius * Mathf.Cos(radians) * 3.5);
            float y = (float)(centerPosition.y + radius * Mathf.Sin(radians) * 3.5);

            // Instantiate Template prefab and set its position
            Image templateInstance = Instantiate(TemplatePrefab, transform); // Instantiate under ReportCode GameObject
            //RectTransform templateRectTransform = templateInstance.rectTransform;
            //templateRectTransform.anchoredPosition = new Vector2(x, y);
            templateInstance.transform.position = new Vector2(x, y);
 
            // Add instantiated Template to list
            templateInstances.Add(templateInstance);
        }

        for (int i = 0; i < leftMissAngleList.Count; i++)
        {
            
            float targetAngle = leftMissAngleList[i];

            // Calculate position on circumference based on angle
            float radians = (float)(Mathf.Abs(Mathf.PI - (targetAngle * Mathf.Deg2Rad - Mathf.PI / 2)));
            float x = (float)(centerPosition.x + radius * Mathf.Cos(radians) * 3.5);
            float y = (float)(centerPosition.y + radius * Mathf.Sin(radians) * 3.5);

            // Instantiate Template prefab and set its position
            Image templateInstance = Instantiate(TemplatePrefab, transform); // Instantiate under ReportCode GameObject
            //RectTransform templateRectTransform = templateInstance.rectTransform;
            //templateRectTransform.anchoredPosition = new Vector2(x, y);
            templateInstance.color = Color.red;
            templateInstance.transform.position = new Vector2(x, y);

            // Add instantiated Template to list
            templateInstances.Add(templateInstance);
        }

        for (int i = 0; i < rightMissAngleList.Count; i++)
        {
            
            float targetAngle = rightMissAngleList[i];

            // Calculate position on circumference based on angle
            float radians = (float)(targetAngle * Mathf.Deg2Rad + (3 * Mathf.PI / 2));
            float x = (float)(centerPosition.x + radius * Mathf.Cos(radians) * 3.5);
            float y = (float)(centerPosition.y + radius * Mathf.Sin(radians) * 3.5);

            // Instantiate Template prefab and set its position
            Image templateInstance = Instantiate(TemplatePrefab, transform); // Instantiate under ReportCode GameObject
            //RectTransform templateRectTransform = templateInstance.rectTransform;
            //templateRectTransform.anchoredPosition = new Vector2(x, y);
            templateInstance.color = Color.red;
            templateInstance.transform.position = new Vector2(x, y);

            // Add instantiated Template to list
            templateInstances.Add(templateInstance);
        }
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }
}
