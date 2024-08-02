using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashingColor : MonoBehaviour
{
    public TMP_Text title;
    private Color color1 = Color.red;
    private Color color2 = Color.white;
 
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(FlashText());  
    }

    private IEnumerator FlashText()
    {
        while (true)
        {
            
            title.color = title.color == color1 ? color2 : color1;
            
            yield return new WaitForSeconds(1);
        }

        
    }
}
