using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose}

public class BattleMgr : MonoBehaviour
{
    string lastScene;

    public GameObject[] enemies;
    public int enemyShade;

    public BattleState state;

    public TextMeshProUGUI playerName;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI dialougeBox;

    public GameObject player;
    public Shade playerCreature, enemyCreature;
    public Meter playerHealth, playerEnergy, enemyHealth, enemyEnergy;
    public GameObject enemy;
    public GameObject[] backgrounds;
    public GameObject playerHUD, enemyHUD;

    public Transform playerPosition;
    public Transform enemyPosition;

    //public string nameOfEnemy;
    public bool playerTurn = false;

    public CombatMenu combatMenu;

    public SceneLoader loader;

    private void Awake()
    {
        playerHUD.SetActive(false);
        enemyHUD.SetActive(false);
        enemyShade = Random.Range(0, 7);
        Debug.Log(enemyShade.ToString());
        enemy = enemies[enemyShade];
        enemies[enemyShade].SetActive(true);
        enemyCreature = enemies[enemyShade].GetComponent<Shade>();
        state = BattleState.BattleStart;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        if(playerCreature.health <= 0)
        {
            state = BattleState.Lose;
            StartCoroutine(BattleLoss());
        }
        if(enemyCreature.health <= 0)
        {
            state = BattleState.Win;
            StartCoroutine(BattleWin());
        }
    }

    IEnumerator SetupBattle()
    {
        Debug.Log("Setup Battle");

        int battleLocation = PlayerPrefs.GetInt("battleLocation");
        backgrounds[battleLocation].SetActive(true);

        GameObject playerGO = Instantiate(player, playerPosition);
        GameObject enemyGO = Instantiate(enemy, enemyPosition);


        dialougeBox.text = "Enemy " + enemyCreature.name + " appears!";
        playerName.text = playerCreature.name;
        enemyName.text = enemyCreature.name;

        yield return new WaitForSeconds(1f);
        dialougeBox.text = "The Battle Begins...";
        //yield return new WaitForSeconds(1f);

        combatMenu.playerCreature.SetupHealthBar();
        combatMenu.playerCreature.SetupEnergyBar();
        combatMenu.enemyCreature.SetupHealthBar();
        combatMenu.enemyCreature.SetupEnergyBar();

        playerHUD.SetActive(true);
        enemyHUD.SetActive(true);

        yield return new WaitForSeconds(1f);

        state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        combatMenu.actionButton.gameObject.SetActive(true);
        combatMenu.useItemButton.gameObject.SetActive(true);
        combatMenu.fleeButton.gameObject.SetActive(true);

        Debug.Log("Player Turn");
        dialougeBox.text = "Player's turn";
        
        yield return null;
    }

    IEnumerator EnemyTurn()
    {
        Debug.Log("Enemy Turn");
        //yield return new WaitForSeconds(1f);
        dialougeBox.text = "Enemy's turn";

        yield return new WaitForSeconds(1f);

        int enemyAction = Random.Range(1, 4);
        if (enemyCreature.charged)
        {
            enemyAction = 3;
        }
        if (enemyCreature.energy < 10)
        {
            enemyAction = 1;
        }
        switch (enemyAction)
        {
            case 1:
                combatMenu.EnemyDefend();
                break;
            case 2:
                combatMenu.EnemyCharge();
                break;
            case 3:
                combatMenu.EnemyAttack();
                break;
            default:
                Debug.Log("Error: invalid value for: enemyAction");
                break;
        }

        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerTurn;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator BattleWin()
    {
        dialougeBox.text = "Enemy " + enemyCreature.name + " was defeated";
        yield return new WaitForSeconds(3f);
        EndBattle();
    }

    IEnumerator BattleLoss()
    {
        dialougeBox.text = "You were defeated";
        yield return new WaitForSeconds(3f);
        EndBattle();
    }

    public void StartEnemyTurn()
    {
        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
        combatMenu.actionButton.gameObject.SetActive(false);
        combatMenu.useItemButton.gameObject.SetActive(false);
        combatMenu.fleeButton.gameObject.SetActive(false);
    }

    public void EndBattle()
    {
        lastScene = PlayerPrefs.GetString("sceneLoadedFrom");
        loader.LoadScene(lastScene);
        //SceneManager.LoadScene(lastScene);
    }
}
