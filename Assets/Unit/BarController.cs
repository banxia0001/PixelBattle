using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    private float valueNow;
    public float valueShould;
    private float valueMax;
    public Image healthBarImage;
    public void SetValue_Initial(float valueCurrent, float value)
    {
        float ratio = valueCurrent / value;
        slider.value = ratio;
        valueShould = ratio;
        valueNow = ratio;
    }

    public void SetValue(float valueCurrent, float valueMax)
    {
        float ratio = valueCurrent / valueMax;
        valueShould = ratio;
    }

    public void SetValue(float valueRatio)
    {
        valueShould = valueRatio;
    }
    void FixedUpdate()
    {
        slider.value = valueNow;
        if (valueNow > valueShould) valueNow -= 0.1f * Time.fixedDeltaTime;
        if (valueNow < valueShould) valueNow += 0.1f * Time.fixedDeltaTime;
    }
}
