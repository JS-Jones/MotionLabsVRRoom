using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessSceneGeneration : MonoBehaviour
{
    public GameObject[] sections;
    public int zPos = 100;

    public int sectionNum;


    public string tag = "Section";


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            // Randomly create a new section
            sectionNum = Random.Range(0, 3);
            Instantiate(sections[sectionNum], new Vector3(0, 0, zPos), Quaternion.identity);
        }
    }
}
