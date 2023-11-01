using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose}

public class BattleMgr : MonoBehaviour
{
    string sceneName;

    public BattleState state;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI dialougeBox;

    public GameObject player;
    public GameObject enemy;
    public GameObject[] backgrounds;

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

        int battleLocation = PlayerPrefs.GetInt("battleLocation");
        backgrounds[battleLocation].SetActive(true);

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

    void BattleWin()
    {
        dialougeBox.text = "Enemy [name] was defeated";
    }

    void BattleLoss()
    {
        dialougeBox.text = "You were defeated";
    }

    public void StartEnemyTurn()
    {
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    public void EndBattle()
    {
        sceneName = "OverWorld";
        SceneManager.LoadScene(sceneName);
    }
}
