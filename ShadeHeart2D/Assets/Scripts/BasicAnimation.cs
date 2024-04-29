using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimation : MonoBehaviour
{
    public GameObject shadePosition;

    private float t;
    public float rate = 3.5f;
    public float amplitude = 0.075f;
    public float variance = 1f;

    // Update is called once per frame
    void Update()
    {
        t += (Time.deltaTime * rate);
        shadePosition.transform.localPosition = new Vector3(0, (amplitude * Mathf.Sin(variance*t)), 0);
    }
}
