using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Meter : MonoBehaviour
{

    public Slider meter;
    public TMP_Text meterText;

    public void SetValue(float currentValue)
    {
        meter.value = currentValue;
        meterText.text = meter.value.ToString("F0") + "/" + meter.maxValue.ToString();        
    }

    public void ChangeValue(float currentValue)
    {
        /*
        meter.value = currentValue;
        meterText.text = meter.value.ToString("F0");
        */
        StartCoroutine(ChangeingValue(currentValue));
    }

    public void SetMaxValue(float maxValue)
    {
        meter.maxValue = maxValue;
        meter.value = maxValue;
        meterText.text = maxValue.ToString("F0") + "/" + maxValue.ToString();
    }

    public void SetValueMenu(float currentValue)
    {
        meter.value = currentValue;
        meterText.text = meter.value.ToString("F0");
    }

    public void SetMaxValueMenu(float maxValue)
    {
        meter.maxValue = maxValue;
        meter.value = maxValue;
        meterText.text = meter.value.ToString("F0");
    }

    IEnumerator ChangeingValue(float currentValue)
    {
        float incrementor = meter.maxValue / 150;

        if (meter.value > currentValue)
        {
            while (meter.value > currentValue)
            {
                meter.value -= incrementor;
                yield return null;

                meterText.text = meter.value.ToString("F0") + "/" + meter.maxValue.ToString();
            }
            meter.value = currentValue;
        }
        else
        {
            while (meter.value < currentValue)
            {
                meter.value += incrementor;
                yield return null;

                meterText.text = meter.value.ToString("F0") + "/" + meter.maxValue.ToString();
            }
            meter.value = currentValue;
        }
        
        meterText.text = meter.value.ToString("F0") + "/" + meter.maxValue.ToString();
        yield return null;
    }
}