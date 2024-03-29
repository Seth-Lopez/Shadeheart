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
    [SerializeField] string lastScene;
    [SerializeField] SceneLoader loader;

    //Array of GameObjects with a Shade component and the index of the player that should be used
    public GameObject[] playerShades;
    public int playerIndex;

    //Array of GameObjects with a Shade component and the index of the enemy that should be used
    public GameObject[] enemies;
    public int enemyIndex;

    [SerializeField] bool randomizeNumEnemies;
    [SerializeField] bool randomizeEnemy;

    [SerializeField] int numEnemies = 1;
    [SerializeField] int maxEnemies = 2;
    int previousEnemy = -1;

    [SerializeField] int enemyLevelRange = 2;

    public BattleState state;
    //public bool playerTurn = false;

    //UI variables
    [SerializeField] TextMeshProUGUI playerName, enemyName, playerLevel, enemyLevel, dialougeBox;
    public GameObject player, enemy;
    public Shade playerCreature, enemyCreature;
    [SerializeField] Meter playerHealth, playerEnergy, enemyHealth, enemyEnergy;
    [SerializeField] GameObject playerHUD, enemyHUD;

    [SerializeField] Button[] skillButtons;

    public GameObject combatSelectedButton, skillSelectedButton, skillCloseButton, partyOpenButton, partyCloseButton, skillMenu, combatMenu;

    //Array of background sprites
    [SerializeField] GameObject[] backgrounds;

    //Position of Player and Enemy on screen
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform enemyPosition;

    [SerializeField] CombatMenu combatMenuScript;
    [SerializeField] PartyMenu partyMenuScript;

    [SerializeField] GameObject combatMenuMgr;

    private void Awake()
    {
        
    }

    public void Start()
    {
        //Start with HUDs disabled
        playerHUD.SetActive(false);
        enemyHUD.SetActive(false);

        combatMenu.SetActive(false);
        skillMenu.SetActive(false);

        playerIndex = PlayerPrefs.GetInt("playerShadeIndex");
        Debug.Log("playerIndex: " + playerIndex.ToString());

        SetShade(ref player, playerShades, playerIndex, ref playerCreature);
        combatMenuScript.SetPlayer();

        //randomly adds a second enemy to some battles
        if (randomizeNumEnemies)
        {
            numEnemies = Random.Range(1, maxEnemies + 1);
            Debug.Log("numEnemies: " + numEnemies.ToString());
        }

        //selects enemy
        selectEnemy();
        SetSkills(ref playerCreature, true);//set player skills after enemy is randomized so the skills target properly
        combatMenuScript.SetEnemy();
        //Start Battle
        state = BattleState.BattleStart;
        StartCoroutine(SetupBattle());

        combatMenuMgr.SetActive(true);

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
        SetupPlayer();
        SetupEnemy();

        yield return new WaitForSeconds(1f);

        TurnOrder();
    }

    IEnumerator PlayerTurn()
    {
        //checks if enemy can be stunned
        if (playerCreature.wasStunned)
        {
            playerCreature.wasStunned = false;
        }

        //checks if enemy is stunned
        if (enemyCreature.isStunned)
        {
            enemyCreature.isStunned = false;
            enemyCreature.wasStunned = true;

            Debug.Log("Player Turn");
            dialougeBox.text = playerCreature.name + " is stunned...";
            yield return new WaitForSeconds(1f);

            state = BattleState.EnemyTurn;
            StartEnemyTurn();
            yield return null;
        }
        else
        {
            //activates player's buttons
            combatMenu.SetActive(true);
            OpenCombatMenu();

            Debug.Log("Player Turn");
            dialougeBox.text = "Player's turn";

            yield return null;
        }
    }

    //Determines enemy behavior
    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);
        
        combatMenuScript.descriptionObject.SetActive(false);

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

            //checks if enemy can be stunned
            if (enemyCreature.wasStunned)
            {
                enemyCreature.wasStunned = false;
            }

            //checks if enemy is stunned
            if (enemyCreature.isStunned)
            {
                acted = true;
                enemyCreature.isStunned = false;
                enemyCreature.wasStunned = true;

                dialougeBox.text = enemyCreature.name + " is stunned...";
            }

            while (!acted) 
            {
                enemyAction = Random.Range(0, skillButtons.Length +2);//2 is added to give a higher chance of enemies doing a basic attack
                switch (enemyAction)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        acted = combatMenuScript.UseSkill(enemyCreature.activeSkills[enemyAction]);
                        break;
                    default:
                        combatMenuScript.EnemyAttack();
                        acted = true;
                        break;
                }
            }

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
        combatMenu.SetActive(false);

        state = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    //Determines who acts first
    public void TurnOrder()
    {
        if (playerCreature.Speed > enemyCreature.Speed)
        {
            state = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }

        else if (playerCreature.Speed < enemyCreature.Speed)
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

        enemyCreature.level += (Random.Range(0, enemyLevelRange + 1));

        SetSkills(ref enemyCreature, false);
    }

    public void selectEnemy()
    {
        if (randomizeEnemy)
        {
            RandomizeEnemy();
        }
        else
        {
            enemyIndex = PlayerPrefs.GetInt("enemyID");
            Debug.Log("Enemy index: " + enemyIndex.ToString());
            SetShade(ref enemy, enemies, enemyIndex, ref enemyCreature);

            enemyCreature.level += (Random.Range(0, enemyLevelRange + 1));

            SetSkills(ref enemyCreature, false);
        }
    }

    public void SetShade(ref GameObject shadeLocation, GameObject[]shades, int shadeIndex, ref Shade activeShade)
    {   
        shadeLocation = shades[shadeIndex];
        shades[shadeIndex].SetActive(true);
        activeShade = shades[shadeIndex].GetComponent<Shade>();
    }

    public void SetSkills(ref Shade activeShade, bool isPlayer)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            SetupSkill(ref activeShade.activeSkills[i], isPlayer, activeShade);
            if (isPlayer)
            {
                SetupSkillButton(ref skillButtons[i], i, activeShade);
            }
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
            skillButton.onClick.RemoveAllListeners();
            skillButton.onClick.AddListener(delegate { skillMenu.SetActive(false); });
            skillButton.onClick.AddListener(delegate { combatMenuScript.UseSkill(activeShade.activeSkills[skillIndex]); });

            skillButton.onClick.AddListener(delegate { combatMenuScript.ChangeDescription(activeShade.activeSkills[skillIndex].description); });
            skillButton.onClick.AddListener(delegate { combatMenuScript.descriptionObject.SetActive(false); });
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

    public void SetupPlayer()
    {
        playerHUD.SetActive(false);
        playerName.text = playerCreature.name;
        playerLevel.text = "LV: " + playerCreature.level.ToString();
        combatMenuScript.playerCreature.SetupHealthBar();
        combatMenuScript.playerCreature.SetupEnergyBar();
        playerHUD.SetActive(true);
        playerCreature.UpdateHealth(0);
        playerCreature.UpdateEnergy(0);
    }

    public void SetupEnemy()
    {
        enemyHUD.SetActive(false);
        enemyName.text = enemyCreature.name;
        enemyLevel.text = "LV: " + enemyCreature.level.ToString();
        combatMenuScript.enemyCreature.SetupHealthBar();
        combatMenuScript.enemyCreature.SetupEnergyBar();
        enemyHUD.SetActive(true);
        enemyCreature.UpdateHealth(0);
        enemyCreature.UpdateEnergy(0);
    }

    public void OpenCombatMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(combatSelectedButton);
    }

    public void OpenSkillMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(skillSelectedButton);
    }

    public void CloseSkillMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(skillCloseButton);
    }

    public void OpenParty()
    {
        partyMenuScript.OpenPartyMenu();
    }

    public void ClosePartyMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(partyCloseButton);
    }
}
