using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketMayhemTracker : MonoBehaviour
{
    public static int count = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            count++;
        }
    }

    public int GetCount()
    {
        return count;
    }
}
