using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Meter : MonoBehaviour
{

    public Slider meter;
    public TMP_Text meterText;

    public void SetValue(float currentValue, float maxValue)
    {
        meter.value = currentValue;
        meterText.text = currentValue.ToString("F0") + '/' + maxValue.ToString();
    }

    public void SetMaxValue(float maxValue)
    {
        //meter.GetComponent<Slider>().maxValue = maxValue;
        meter.maxValue = maxValue;
        meter.value = maxValue;
        meterText.text = maxValue.ToString() + '/' + maxValue.ToString();
    }
}