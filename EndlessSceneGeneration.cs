using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSceneGeneration : MonoBehaviour
{
    public GameObject[] sections;
    public int zPos = 100;
    // public bool creatingSection = false;
    public int sectionNum;


    // void Start()
    // {
        
    // }

    // void Update()
    // {
    //     if (!creatingSection) 
    //     {
    //         creatingSection = true;
    //         StartCoroutine(GenerateSection());
    //     }
    // }

    // IEnumerator GenerateSection() 
    // {
    //     sectionNum = Random.Range(0, 3);
    //     Instantiate(section[sectionNum], new Vector3(0, 0, zPos), Quaternion.identity);
    //     yield return new WaitForSeconds(2); // FIXME: change so that section is generated based on where the current one is, not time
    //     creatingSection = false;
    // }

    public string tag = "Section";


    // private void OnCollisionEnter(Collision collision) // needs both objects to have rigidbody
    private void OnTriggerEnter(Collider other)
    {
        // if (collision.gameObject.CompareTag(section))
        if (other.gameObject.CompareTag(tag))
        {
            print("Collision Detected!");
            sectionNum = Random.Range(0, 3);
            Instantiate(sections[sectionNum], new Vector3(0, 0, zPos), Quaternion.identity);
        }
    }
}
