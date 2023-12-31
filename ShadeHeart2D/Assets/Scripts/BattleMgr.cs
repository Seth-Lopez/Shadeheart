using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose}

public class BattleMgr : MonoBehaviour
{
    //For loading scenes
    string lastScene;
    public SceneLoader loader;

    //Array of GameObjects with a Shade component and the index of the enemy that should be used
    public GameObject[] enemies;
    public int enemyIndex;

    public bool randomizeNumEnemies;

    int numEnemies = 1;
    int previousEnemy = -1;

    public BattleState state;
    //public bool playerTurn = false;

    //UI variables
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI enemyName;
    public TextMeshProUGUI dialougeBox;
    public GameObject player, enemy;
    public Shade playerCreature, enemyCreature;
    public Meter playerHealth, playerEnergy, enemyHealth, enemyEnergy;
    public GameObject playerHUD, enemyHUD;

    public GameObject combatSelectedButton, actionSelectedButton;

    //Araay of background sprites
    public GameObject[] backgrounds;

    //Position of Player and Enemy on screen
    public Transform playerPosition;
    public Transform enemyPosition;

    public CombatMenu combatMenu;

    private void Awake()
    {
        //Start with HUDs disabled
        playerHUD.SetActive(false);
        enemyHUD.SetActive(false);

        combatMenu.actionButton.gameObject.SetActive(false);
        combatMenu.useItemButton.gameObject.SetActive(false);
        combatMenu.fleeButton.gameObject.SetActive(false);

        //randomly adds a second enemy to some battles
        if (randomizeNumEnemies)
        {
            numEnemies = Random.Range(1, 3);
            Debug.Log("numEnemies: " + numEnemies.ToString());
        }

        //Randomly select enemy
        RandomizeEnemy();
        combatMenu.SetEnemy();
        //Start Battle
        state = BattleState.BattleStart;
        StartCoroutine(SetupBattle());
    }

    public void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(combatSelectedButton);
    }

    IEnumerator SetupBattle()
    {
        Debug.Log("Setup Battle");

        //Sets the correct background
        int battleLocation = PlayerPrefs.GetInt("battleLocation");
        backgrounds[battleLocation].SetActive(true);

        //GameObject playerGO = Instantiate(player, playerPosition);
        //GameObject enemyGO = Instantiate(enemy, enemyPosition);

        
        dialougeBox.text = "Enemy " + enemyCreature.name + " appears!";
        yield return new WaitForSeconds(2f);

        dialougeBox.text = "The Battle Begins...";

        //Seting up HUDs
        playerName.text = playerCreature.name;
        combatMenu.playerCreature.SetupHealthBar();
        combatMenu.playerCreature.SetupEnergyBar();
        SetupEnemy();

        playerHUD.SetActive(true);

        yield return new WaitForSeconds(1f);

        TurnOrder();
    }

    IEnumerator PlayerTurn()
    {
        //activates player's buttons
        combatMenu.actionButton.gameObject.SetActive(true);
        combatMenu.useItemButton.gameObject.SetActive(true);
        combatMenu.fleeButton.gameObject.SetActive(true);
        OpenCombatMenu();

        Debug.Log("Player Turn");
        dialougeBox.text = "Player's turn";
        
        yield return null;
    }

    //Determines enemy behavior
    IEnumerator EnemyTurn()
    {
        //checks if enemy was defeted
        if (enemyCreature.health <= 0)
        {
            if (numEnemies > 1)
            {
                dialougeBox.text = "Enemy " + enemyCreature.name + " was defeated";
                yield return new WaitForSeconds(1f);

                enemies[enemyIndex].SetActive(false);
                RandomizeEnemy();
                combatMenu.SetEnemy();
                SetupEnemy();

                dialougeBox.text = "Enemy " + enemyCreature.name + " appears!";
                yield return new WaitForSeconds(1f);

                numEnemies -= 1;
                Debug.Log("Enemies remaining: " + numEnemies.ToString());
                TurnOrder();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return null;
                state = BattleState.Win;
                StartCoroutine(BattleWin());
            }
        }
        else
        {
            Debug.Log("Enemy Turn");
            dialougeBox.text = "Enemy's turn";

            yield return new WaitForSeconds(1f);

            int enemyAction = Random.Range(1, 4);
            if (enemyCreature.charged)
            {
                enemyAction = 3;
            }
            if (enemyCreature.energy < combatMenu.chargeCost)
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
            //checks if player was defeted
            if (playerCreature.health <= 0)
            {
                yield return null;
                state = BattleState.Lose;
                StartCoroutine(BattleLoss());
            }

            yield return new WaitForSeconds(1f);
            state = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }
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
        //deactivates player's buttons
        combatMenu.actionButton.gameObject.SetActive(false);
        combatMenu.useItemButton.gameObject.SetActive(false);
        combatMenu.fleeButton.gameObject.SetActive(false);

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    //Determines who acts first
    public void TurnOrder()
    {
        if (playerCreature.speed > enemyCreature.speed)
        {
            state = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }

        else if (playerCreature.speed < enemyCreature.speed)
        {
            StartEnemyTurn();
        }
        else
        {
            if ((Random.Range(0, 2)) == 0)
            {
                state = BattleState.PlayerTurn;
                StartCoroutine(PlayerTurn());
            }
            else
            {
                StartEnemyTurn();
            }
        }
    }

    public void EndBattle()
    {
        lastScene = PlayerPrefs.GetString("sceneLoadedFrom");
        loader.LoadScene(lastScene);
    }

    public void RandomizeEnemy()
    {
        do
        {
            enemyIndex = Random.Range(0, 7);
        }
        while (enemyIndex == previousEnemy);
        previousEnemy = enemyIndex;
        Debug.Log("Enemy index: " + enemyIndex.ToString());
        enemy = enemies[enemyIndex];
        enemies[enemyIndex].SetActive(true);
        enemyCreature = enemies[enemyIndex].GetComponent<Shade>();
    }

    public void SetupEnemy()
    {
        enemyName.text = enemyCreature.name;
        combatMenu.enemyCreature.SetupHealthBar();
        combatMenu.enemyCreature.SetupEnergyBar();
        enemyHUD.SetActive(true);
    }

    public void OpenCombatMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(combatSelectedButton);
    }

    public void OpenAcionMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(actionSelectedButton);
    }
}
