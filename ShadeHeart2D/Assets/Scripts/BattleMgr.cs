using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose}

public class BattleMgr : MonoBehaviour
{
    public BattleState state;

    public GameObject player;
    public GameObject enemy;

    public Transform playerPosition;
    public Transform enemyPosition;
    
    void Start()
    {
        state = BattleState.BattleStart;
        SetupBattle();
    }
    
    void SetupBattle()
    {
        Debug.Log("Setup Battle");
        Instantiate(player, playerPosition);
        Instantiate(enemy, enemyPosition);

        state = BattleState.PlayerTurn;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        Debug.Log("Player Turn");
    }

    public void EndBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
