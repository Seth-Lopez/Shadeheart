using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    public static bool spawned1;
    public static bool spawned2;
    public static bool spawned3;
    public static bool spawned4;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("reset") == 1)
        {
            enemy1.GetComponent<EnemyTracker1>().SetDefeated(false);
            enemy2.GetComponent<EnemyTracker2>().SetDefeated(false);
            enemy3.GetComponent<EnemyTracker3>().SetDefeated(false);
            enemy4.GetComponent<EnemyTracker4>().SetDefeated(false);
        }

        spawned1 = false;
        spawned2 = false;
        spawned3 = false;
        spawned4 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned1 && enemy1.GetComponent<EnemyTracker1>().GetDefeated())
        {
            Instantiate(enemy1);
            spawned1 = true;
        }
        if (!spawned2 && enemy2.GetComponent<EnemyTracker2>().GetDefeated())
        {
            Instantiate(enemy2);
            spawned2 = true;
        }
        if (!spawned3 && enemy3.GetComponent<EnemyTracker3>().GetDefeated())
        {
            Instantiate(enemy3);
            spawned3 = true;
        }
        if (!spawned4 && enemy4.GetComponent<EnemyTracker4>().GetDefeated())
        {
            Instantiate(enemy4);
            spawned4 = true;
        }
    }
}
