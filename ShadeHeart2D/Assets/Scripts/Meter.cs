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
        meterText.text = meter.value.ToString("F0") + '/' + maxValue.ToString();
    }

    public void SetMaxValue(float maxValue)
    {
        meter.maxValue = maxValue;
        meter.value = maxValue;
        meterText.text = maxValue.ToString() + '/' + maxValue.ToString();
    }

    IEnumerator ChangeValue(float currentValue, float maxValue)
    {
        float incrementor = 0.5f;

        if (meter.value > currentValue)
        {
            incrementor = -incrementor;
        }
        while (meter.value != currentValue)
        {
            meter.value += incrementor;
            meterText.text = meter.value.ToString("F0") + '/' + maxValue.ToString();
            yield return null;
        }
        yield return null;
    }
}