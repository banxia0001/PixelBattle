using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public Slider slider;
    private float valueNow;
    private float valueShould;
    private float valueMax;
    public Image healthBarImage;
    public void SetValue_Initial(int valueCurrent, int value)
    {
        valueNow = (float)valueCurrent;
        valueMax = (float)value;
        slider.maxValue = valueMax;
        slider.value = valueNow;
        valueShould = valueNow;
    }

    public void SetValue(int valueCurrent, int valueMax)
    {
        valueShould = (float)valueCurrent;
    }

    void FixedUpdate()
    {
        slider.value = valueNow;
        if (valueNow > valueShould) valueNow -= valueMax / 30;
        if (valueNow < valueShould) valueNow += valueMax / 30;
    }
}