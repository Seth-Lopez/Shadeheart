using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose}

public class BattleMgr : MonoBehaviour
{
    public BattleState state;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI dialougeBox;

    public GameObject player;
    public GameObject enemy;

    public Transform playerPosition;
    public Transform enemyPosition;
    
    void Start()
    {
        state = BattleState.BattleStart;
        StartCoroutine(SetupBattle());
    }
    
    IEnumerator SetupBattle()
    {
        Debug.Log("Setup Battle");
        GameObject playerGO = Instantiate(player, playerPosition);
        GameObject enemyGO = Instantiate(enemy, enemyPosition);

        playerName.text = "Player";
        enemyName.text = "Enemy";
        dialougeBox.text = "The Battle Begins...";

        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        Debug.Log("Player Turn");
        dialougeBox.text = "Player's turn";
        
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        dialougeBox.text = "Enemy's turn";

        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    public void EndBattle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void StartEnemyTurn()
    {
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }
}
