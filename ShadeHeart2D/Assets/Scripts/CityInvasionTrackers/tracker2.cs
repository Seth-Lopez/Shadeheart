using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tracker2 : MonoBehaviour
{
    public static bool defeated = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            defeated = true;
        }
    }
    public bool GetDefeated()
    {
        return defeated;
    }
    public void SetDefeated(bool value)
    {
        defeated = value;
    }
}
