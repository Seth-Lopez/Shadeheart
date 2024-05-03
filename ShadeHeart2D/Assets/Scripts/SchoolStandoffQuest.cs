using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//3 Shades are at the school one at a time defeat them all to complete the quest

public class SchoolStandoffQuest : MonoBehaviour
{
    public bool questStarted;
    public bool questFinished;

    public static bool spawned1;
    public static bool spawned2;
    public static bool spawned3;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject bossEnemy;

    public void Start()
    {
        if (PlayerPrefs.GetInt("reset") == 1)
        {
            questStarted = false;
            questFinished = false;
            enemy1.GetComponent<SchoolQuestTracker>().SetCount(0);
        }
        spawned1 = false;
        spawned2 = false;
        spawned3 = false;
    }
    public void Update()
    {
        if (questStarted && !questFinished)
        {
            Debug.Log($"count: {enemy1.GetComponent<SchoolQuestTracker>().GetCount()}");
            if (!spawned1 && enemy1.GetComponent<SchoolQuestTracker>().GetCount() == 0)
            {
                Instantiate(enemy1);
                spawned1 = true;
            }
            if (!spawned2 && enemy2.GetComponent<SchoolQuestTracker>().GetCount() == 1)
            {
                Instantiate(enemy2);
                spawned2 = true;
            }
            if (!spawned3 && bossEnemy.GetComponent<SchoolQuestTracker>().GetCount() == 2)
            {
                Instantiate(bossEnemy);
                spawned3 = true;
            }
            if (!spawned3 && bossEnemy.GetComponent<SchoolQuestTracker>().GetCount() == 3)
            {
                questFinished = true;
            }
        }
    }

    public void SetupQuest()
    {
        Debug.Log($"count: {enemy1.GetComponent<SchoolQuestTracker>().GetCount()}");
        if (!spawned1 && enemy1.GetComponent<SchoolQuestTracker>().GetCount() == 0)
        {
            Instantiate(enemy1);
            spawned1 = true;
        }
        if (!spawned2 && enemy2.GetComponent<SchoolQuestTracker>().GetCount() == 1)
        {
            Instantiate(enemy2);
            spawned2 = true;
        }
        if (!spawned3 && bossEnemy.GetComponent<SchoolQuestTracker>().GetCount() == 2)
        {
            Instantiate(bossEnemy);
            spawned3 = true;
        }
        if (!spawned3 && bossEnemy.GetComponent<SchoolQuestTracker>().GetCount() == 3)
        {
            questFinished = true;
        }
        /*if (enemy1.GetComponent<EnemyChecker>().GetDefeated() == true && enemy2.GetComponent<EnemyChecker>().GetDefeated() == true && bossEnemy.GetComponent<EnemyChecker>().GetDefeated() == true)
        {
            questFinished = true;
        }
        if (bossEnemy.GetComponent<EnemyChecker>().GetDefeated() == false && enemy1.GetComponent<EnemyChecker>().GetDefeated() == true && enemy2.GetComponent<EnemyChecker>().GetDefeated() == true && spawnedBoss == false)
        {
            Instantiate(bossEnemy);
            spawnedBoss = true;
        }
        if (enemy2.GetComponent<EnemyChecker>().GetDefeated() == false && enemy1.GetComponent<EnemyChecker>().GetDefeated() == true && spawned2 == false)
        {
            Instantiate(enemy2);
            spawned2 = true;
        }
        if (enemy1.GetComponent<EnemyChecker>().GetDefeated() == false && spawned1 == false)
        {
            Instantiate(enemy1);
            spawned1 = true;
        }*/
    }
}
