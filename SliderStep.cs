using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderStep : MonoBehaviour
{
    private Slider slider;
    public TMP_Text Left;
    public TMP_Text Right;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChanged(); });
    }

    void ValueChanged()
    {
        // Round the slider value to the nearest step (0.1 in this case)
        float step = 0.01f;
        slider.value = Mathf.Round(slider.value / step) * step;
        Left.text = "Left: " + (100 - (slider.value * 100)).ToString("F0") + "%";
        Right.text = "Right: " + ((slider.value * 100)).ToString("F0") + "%";

    }
}
