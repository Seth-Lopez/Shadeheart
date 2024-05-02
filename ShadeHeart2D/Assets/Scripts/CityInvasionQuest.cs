using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 9 shades and a boss apear throughout the city, the boss in infront of the hospital, only the boss needs to be defeated to finish the quest

public class CityInvasionQuest : MonoBehaviour
{
    public bool questStarted;
    public bool questFinished;

    public bool spawned1;
    public bool spawned2;
    public bool spawned3;

    public bool spawned4;
    public bool spawned5;
    public bool spawned6;

    public bool spawned7;
    public bool spawned8;
    public bool spawned9;
    
    public bool spawnedBoss;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;
   
    public GameObject enemy7;
    public GameObject enemy8;
    public GameObject enemy9;
    
    public GameObject enemyBoss;

    void Update()
    {
        if (questStarted && !questFinished && !enemyBoss.GetComponent<CityInvasionTracker>().GetDefeated())
        {
            if (!spawned1 && !enemy1.GetComponent<tracker1>().GetDefeated())
            {
                Instantiate(enemy1);
                spawned1 = true;
            }
            if (!spawned2 && !enemy2.GetComponent<tracker2>().GetDefeated())
            {
                Instantiate(enemy2);
                spawned2 = true;
            }
            if (!spawned3 && !enemy3.GetComponent<tracker3>().GetDefeated())
            {
                Instantiate(enemy3);
                spawned3 = true;
            }
            if (!spawned4 && !enemy4.GetComponent<tracker4>().GetDefeated())
            {
                Instantiate(enemy4);
                spawned4 = true;
            }
            if (!spawned5 && !enemy5.GetComponent<tracker5>().GetDefeated())
            {
                Instantiate(enemy5);
                spawned5 = true;
            }
            if (!spawned6 && !enemy6.GetComponent<tracker6>().GetDefeated())
            {
                Instantiate(enemy6);
                spawned6 = true;
            }
            if (!spawned7 && !enemy7.GetComponent<tracker7>().GetDefeated())
            {
                Instantiate(enemy7);
                spawned7 = true;
            }
            if (!spawned8 && !enemy8.GetComponent<tracker8>().GetDefeated())
            {
                Instantiate(enemy8);
                spawned8 = true;
            }
            if (!spawned9 && !enemy9.GetComponent<tracker9>().GetDefeated())
            {
                Instantiate(enemy9);
                spawned9 = true;
            }
            if (!spawnedBoss && !enemyBoss.GetComponent<CityInvasionTracker>().GetDefeated())
            {
                Instantiate(enemyBoss);
                spawnedBoss = true;
            }
        }
        if (enemyBoss.GetComponent<CityInvasionTracker>().GetDefeated())
        {
            questFinished = true;
        }
    }
}
