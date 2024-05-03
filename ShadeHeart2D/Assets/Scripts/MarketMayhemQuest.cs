using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//4 Shades are at the market one at a time, defeat them all to complete the quest

public class MarketMayhemQuest : MonoBehaviour
{
    public bool questStarted;
    public bool questFinished;

    public static bool spawned1;
    public static bool spawned2;
    public static bool spawned3;
    public static bool spawned4;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("reset") == 1)
        {
            questStarted = false;
            questFinished = false;
            enemy1.GetComponent<MarketMayhemTracker>().SetCount(0);
        }
        spawned1 = false;
        spawned2 = false;
        spawned3 = false;
        spawned4 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (questStarted && !questFinished)
        {
            if (!spawned1 && enemy1.GetComponent<MarketMayhemTracker>().GetCount() == 0)
            {
                Instantiate(enemy1);
                spawned1 = true;
            }
            if (!spawned2 && enemy2.GetComponent<MarketMayhemTracker>().GetCount() == 1)
            {
                Instantiate(enemy2);
                spawned2 = true;
            }
            if (!spawned3 && enemy3.GetComponent<MarketMayhemTracker>().GetCount() == 2)
            {
                Instantiate(enemy3);
                spawned3 = true;
            }
            if (!spawned4 && enemy4.GetComponent<MarketMayhemTracker>().GetCount() == 3)
            {
                Instantiate(enemy4);
                spawned4 = true;
            }
        }
        if (enemy1.GetComponent<MarketMayhemTracker>().GetCount() == 4)
        {
            questFinished = true;
        }
    }
}
