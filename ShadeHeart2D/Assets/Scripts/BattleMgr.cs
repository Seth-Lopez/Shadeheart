using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose }

public class BattleMgr : MonoBehaviour
{
    //For loading scenes
    string lastScene;
    public SceneLoader loader;

    public GameObject[] playerShades;
    public int playerIndex;

    //Array of GameObjects with a Shade component and the index of the enemy that should be used
    public GameObject[] enemies;
    public int enemyIndex;

    public bool randomizeNumEnemies;

    int numEnemies = 1;
    int previousEnemy = -1;

    public BattleState state;
    //public bool playerTurn = false;

    //UI variables
    public TextMeshProUGUI playerName, enemyName, dialougeBox;
    public GameObject player, enemy;
    public Shade playerCreature, enemyCreature;
    public Meter playerHealth, playerEnergy, enemyHealth, enemyEnergy;
    public GameObject playerHUD, enemyHUD;

    public Button skill0, skill1, skill2;

    public GameObject combatSelectedButton, actionSelectedButton, skillSelectedButton, skillMenu, combatMenu;

    //Araay of background sprites
    public GameObject[] backgrounds;

    //Position of Player and Enemy on screen
    public Transform playerPosition;
    public Transform enemyPosition;

    public CombatMenu combatMenuScript;

    private void Awake()
    {
        //Start with HUDs disabled
        playerHUD.SetActive(false);
        enemyHUD.SetActive(false);

        combatMenuScript.actionButton.gameObject.SetActive(false);
        combatMenuScript.useItemButton.gameObject.SetActive(false);
        combatMenuScript.fleeButton.gameObject.SetActive(false);

        playerIndex = PlayerPrefs.GetInt("playerShadeIndex");
        Debug.Log("playerIndex: " + playerIndex.ToString());

        SetShade(ref player, playerShades, playerIndex, ref playerCreature);
        combatMenuScript.SetPlayer();

        //randomly adds a second enemy to some battles
        if (randomizeNumEnemies)
        {
            numEnemies = Random.Range(1, 3);
            Debug.Log("numEnemies: " + numEnemies.ToString());
        }

        //Randomly select enemy
        RandomizeEnemy();
        SetSkills(ref playerCreature, true);//set player skills after enemy is randomized to the skills target properly
        combatMenuScript.SetEnemy();
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
        combatMenuScript.playerCreature.SetupHealthBar();
        combatMenuScript.playerCreature.SetupEnergyBar();
        SetupEnemy();

        playerHUD.SetActive(true);

        yield return new WaitForSeconds(1f);

        TurnOrder();
    }

    IEnumerator PlayerTurn()
    {
        //activates player's buttons
        combatMenu.SetActive(true);
        combatMenuScript.actionButton.gameObject.SetActive(true);
        combatMenuScript.useItemButton.gameObject.SetActive(true);
        combatMenuScript.fleeButton.gameObject.SetActive(true);
        OpenCombatMenu();

        Debug.Log("Player Turn");
        dialougeBox.text = "Player's turn";
        
        yield return null;
    }

    //Determines enemy behavior
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        //checks if enemy was defeted
        if (enemyCreature.health <= 0)
        {
            if (numEnemies > 1)
            {
                dialougeBox.text = "Enemy " + enemyCreature.name + " was defeated";
                yield return new WaitForSeconds(1f);

                enemies[enemyIndex].SetActive(false);
                RandomizeEnemy();
                SetSkills(ref playerCreature, true);//set player skills after enemy is randomized to the skills target properly
                combatMenuScript.SetEnemy();
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

            int enemyAction;
            bool acted = false;
            int i = 0;
            do
            {
                enemyAction = Random.Range(0, 4);
                switch (enemyAction)
                {
                    case 0:
                    case 1:
                    case 2:
                        acted = combatMenuScript.UseSkill(enemyCreature.activeSkills[enemyAction]);
                        break;
                    default:
                        combatMenuScript.EnemyAttack();
                        acted = true;
                        break;
                }
                i++;
            } while (!acted && i < 20);

            /*
            int enemyAction = Random.Range(1, 4);
            if (enemyCreature.isCharged)
            {
                enemyAction = 3;
            }
            if (enemyCreature.energy < combatMenuScript.chargeCost)
            {
                enemyAction = 1;
            }
            switch (enemyAction)
            {
                case 1:
                    combatMenuScript.EnemyDefend();
                    break;
                case 2:
                    combatMenuScript.EnemyCharge();
                    break;
                case 3:
                    combatMenuScript.EnemyAttack();
                    break;
                default:
                    Debug.Log("Error: invalid value for: enemyAction");
                    break;
            }
            */

            //checks if player was defeted
            if (playerCreature.health <= 0)
            {
                yield return new WaitForSeconds(1f);
                state = BattleState.Lose;
                StartCoroutine(BattleLoss());
            }
            else
            {
                yield return new WaitForSeconds(1f);
                state = BattleState.PlayerTurn;
                StartCoroutine(PlayerTurn());
            }
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
        combatMenuScript.actionButton.gameObject.SetActive(false);
        combatMenuScript.useItemButton.gameObject.SetActive(false);
        combatMenuScript.fleeButton.gameObject.SetActive(false);

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
            Debug.Log("enemies.Length: " + (enemies.Length).ToString());
            enemyIndex = Random.Range(0, enemies.Length);
        }
        while (enemyIndex == previousEnemy);
        previousEnemy = enemyIndex;
        Debug.Log("Enemy index: " + enemyIndex.ToString());
        SetShade(ref enemy, enemies, enemyIndex, ref enemyCreature);
        SetSkills(ref enemyCreature, false);
    }

    public void SetShade(ref GameObject shadeLocation, GameObject[]shades, int shadeIndex, ref Shade activeShade)
    {   
        shadeLocation = shades[shadeIndex];
        shades[shadeIndex].SetActive(true);
        activeShade = shades[shadeIndex].GetComponent<Shade>();
    }

    public void SetSkills(ref Shade activeShade, bool isPlayer)
    {
        SetupSkill(ref activeShade.activeSkills[0], isPlayer, activeShade);
        SetupSkill(ref activeShade.activeSkills[1], isPlayer, activeShade);
        SetupSkill(ref activeShade.activeSkills[2], isPlayer, activeShade);

        if (isPlayer)
        {
            SetupSkillButton(ref skill0, 0, activeShade);
            SetupSkillButton(ref skill1, 1, activeShade);
            SetupSkillButton(ref skill2, 2, activeShade);
        }
    }

    public void SetupSkillButton(ref Button skillButton, int skillIndex, Shade activeShade)
    {
        Debug.Log("Setup skill button");
        skillButton.GetComponentInChildren<TextMeshProUGUI>().text = activeShade.activeSkills[skillIndex].name;
        if (skillButton.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            skillButton.interactable = false;
        }
        else
        {
            skillButton.interactable = true;
            skillButton.onClick.AddListener(delegate { skillMenu.SetActive(false); });
            skillButton.onClick.AddListener(delegate { combatMenuScript.UseSkill(activeShade.activeSkills[skillIndex]); });


        }
    }
// figure out how to setup targeting for skills
    public void SetupSkill(ref Skill skill, bool isPlayer, Shade user)
    {
        Debug.Log("setup skill");
        skill.user = user;
        if (skill.isTargetSelf)
        {
            skill.target = skill.user;
        }
        else if (isPlayer)
        {
            skill.target = enemyCreature;
        }
        else
        {
            skill.target = playerCreature;
        }

        if (skill.effectTargetSelf)
        {
            skill.effectTarget = skill.user;
        }
        else if (isPlayer)
        {
            skill.effectTarget = enemyCreature;
        }
        else
        {
            skill.effectTarget = playerCreature;
        }
    }

    public void SetupEnemy()
    {
        enemyName.text = enemyCreature.name;
        combatMenuScript.enemyCreature.SetupHealthBar();
        combatMenuScript.enemyCreature.SetupEnergyBar();
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

    public void OpenSkillMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(skillSelectedButton);
    }
}
