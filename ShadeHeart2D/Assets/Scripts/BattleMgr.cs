using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public enum BattleState { BattleStart, PlayerTurn, EnemyTurn, Win, Lose }

public class BattleMgr : MonoBehaviour
{
    //For loading scenes
    [SerializeField] string lastScene;
    [SerializeField] SceneLoader loader;

    //Array of GameObjects with a Shade component and the index of the player that should be used
    public List<GameObject> playerShades;
    public GameObject[] shades;
    public Shade[] party;
    public int playerIndex;

    public int numPlayerShades;

    //Array of GameObjects with a Shade component and the index of the enemy that should be used
    public List<GameObject> enemies;
    public int enemyIndex;

    [SerializeField] bool randomizeNumEnemies;
    [SerializeField] bool randomizeEnemy;
    [SerializeField] bool intelegentEnemy;

    public int numEnemies = 1;
    [SerializeField] int maxEnemies = 2;
    int previousEnemy = -1;

    [SerializeField] int enemyLevelMin = 6;
    [SerializeField] int enemyLevelMax = 10;

    public int battleTextSpeed;
    public float textStop;

    public BattleState state;
    //public bool playerTurn = false;

    //UI variables
    [SerializeField] TextMeshProUGUI playerName, enemyName, playerLevel, enemyLevel, dialogueBox;
    public GameObject player, enemy;
    public Shade playerCreature, enemyCreature;
    [SerializeField] Meter playerHealth, playerEnergy, enemyHealth, enemyEnergy;
    [SerializeField] GameObject playerHUD, enemyHUD;

    public Button[] skillButtons;

    public GameObject combatSelectedButton, skillSelectedButton, skillCloseButton, partyOpenButton, partyCloseButton, skillMenu, combatMenu;

    //Array of background sprites
    [SerializeField] GameObject[] backgrounds;

    //Position of Player and Enemy on screen
    [SerializeField] Transform playerPosition;
    [SerializeField] Transform enemyPosition;

    public CombatMenu combatMenuScript;
    [SerializeField] PartyMenu partyMenuScript;

    [SerializeField] GameObject combatMenuMgr;
    public GameObject partyMenu;

    //public SkillMgr skillMgr;

    public bool sparePossible = false;
    public bool enemySpared = false;
    public bool enemyCaptured = false;

    public void Start()
    {
        //Start with HUDs disabled
        playerHUD.SetActive(false);
        enemyHUD.SetActive(false);

        combatMenu.SetActive(false);
        skillMenu.SetActive(false);

        playerIndex = PlayerPrefs.GetInt("playerShadeIndex");
        Debug.Log("playerIndex: " + playerIndex.ToString());

        /*if (PartyData.saved)
        {
            LoadPartyData();
        }*/
        /*//---------------------------------------------------------------------------------------------------------------------------------------
        string saveDataPath = Application.persistentDataPath + "/party.sav";
        Debug.Log(saveDataPath);
        if (File.Exists(saveDataPath))
        {
            LoadPartyData();
            //load party
            for (int i = 0; i < party.Length; i++)
            {
                playerShades[i] = shades[party[i].index];

                playerShades[i].GetComponent<Shade>().name = party[i].name;
                playerShades[i].GetComponent<Shade>().level = party[i].level;
                playerShades[i].GetComponent<Shade>().baseMaxHealth = party[i].baseMaxHealth;
                playerShades[i].GetComponent<Shade>().baseMaxEnergy = party[i].baseMaxEnergy;
                playerShades[i].GetComponent<Shade>().baseAttack = party[i].baseAttack;
                playerShades[i].GetComponent<Shade>().baseDefense = party[i].baseDefense;
                playerShades[i].GetComponent<Shade>().baseSpeed = party[i].baseSpeed;
                playerShades[i].GetComponent<Shade>().index = party[i].index;
                playerShades[i].GetComponent<Shade>().exp = party[i].exp;
                playerShades[i].GetComponent<Shade>().lightLevels = party[i].lightLevels;
                playerShades[i].GetComponent<Shade>().darkLevels = party[i].darkLevels;
                playerShades[i].GetComponent<Shade>().levelExp = party[i].levelExp;
                playerShades[i].GetComponent<Shade>().totalExp = party[i].totalExp;
                playerShades[i].GetComponent<Shade>().requiredEXP = party[i].requiredEXP;
                playerShades[i].GetComponent<Shade>().baseRequiredExp = party[i].baseRequiredExp;
                playerShades[i].GetComponent<Shade>().weakness = party[i].weakness;
                for (int j = 0; j < party[i].activeSkills.Count; j++)
                {
                    playerShades[i].GetComponent<Shade>().activeSkills.Add(party[i].activeSkills[j]);
                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------*/

        numPlayerShades = playerShades.Count;
        for (int i = 0; i < playerShades.Count; i++)
        {
            if (playerShades[i].GetComponent<Shade>().health <= 0)
            {
                numPlayerShades--;
            }
        }

        SetShade(ref player, playerShades, playerIndex, ref playerCreature);
        combatMenuScript.SetPlayer();

        //randomly adds a second enemy to some battles
        if (randomizeNumEnemies)
        {
            numEnemies = Random.Range(1, maxEnemies + 1);
            Debug.Log("numEnemies: " + numEnemies.ToString());
        }

        //selects enemy
        SelectEnemy();
        SetSkills(ref playerCreature, true);//set player skills after enemy is randomized so the skills target properly
        combatMenuScript.SetEnemy();
        SetSkills(ref enemyCreature, false);
        //Start Battle
        state = BattleState.BattleStart;
        StartCoroutine(SetupBattle());
        SetSkills(ref playerCreature, true);
        SetSkills(ref enemyCreature, false);

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

        yield return (DisplayingDialogue($"Enemy {enemyCreature.name} appears!"));

        yield return new WaitForSeconds(textStop);

        yield return (DisplayingDialogue($"The Battle Begins..."));
        /*
        dialogueBox.text = "Enemy " + enemyCreature.name + " appears!";
        yield return new WaitForSeconds(2f);

        dialogueBox.text = "The Battle Begins...";
        */
        //Seting up HUDs
        SetupPlayer();
        SetupEnemy();

        yield return new WaitForSeconds(textStop);

        TurnOrder();
    }

    IEnumerator PlayerTurn()
    {
        //checks if enemy can be stunned
        if (playerCreature.wasStunned)
        {
            playerCreature.wasStunned = false;
        }

        //checking secondary effects

        if (playerCreature.isFrozen)
        {
            skillButtons[playerCreature.freezeIndex].interactable = false;
        }

        if (playerCreature.isStunned)
        {
            playerCreature.isStunned = false;
            playerCreature.wasStunned = true;

            Debug.Log("Player Turn");
            yield return (DisplayingDialogue($"{playerCreature.name} is stunned..."));
            //dialogueBox.text = playerCreature.name + " is stunned...";
            yield return new WaitForSeconds(textStop);

            state = BattleState.EnemyTurn;
            StartEnemyTurn();
            yield return null;
        }
        else
        {
            Debug.Log("Player Turn");
            yield return (DisplayingDialogue($"Player's turn"));
            //dialogueBox.text = "Player's turn";

            //activates player's buttons
            combatMenu.SetActive(true);
            OpenCombatMenu();

            yield return new WaitForSeconds(textStop);
        }
    }

    //Determines enemy behavior
    IEnumerator EnemyTurn()
    {
        combatMenu.SetActive(false);

        combatMenuScript.descriptionObject.SetActive(false);

        if (enemySpared)
        {
            Debug.Log("enemy spared");

            int exp = (playerCreature.expCalc(enemyCreature.baseExpYield, enemyCreature.level));
            yield return (DisplayingDialogue($"{playerCreature.name} gains {exp} exp"));
            Debug.Log("test e");
            yield return new WaitForSeconds(textStop);

            Debug.Log("test f");

            playerCreature.UpdateEXP(exp);

            if (numEnemies >= 1)
            {
                enemies[enemyIndex].SetActive(false);
                RandomizeEnemy();
                SetSkills(ref playerCreature, true);//set player skills after enemy is randomized to the skills target properly
                combatMenuScript.SetEnemy();
                SetupEnemy();
                SetSkills(ref enemyCreature, false);

                yield return (DisplayingDialogue($"Enemy {enemyCreature.name} appears!"));
                //dialogueBox.text = "Enemy " + enemyCreature.name + " appears!";
                yield return new WaitForSeconds(textStop);

                numEnemies -= 1;
                Debug.Log("Enemies remaining: " + numEnemies.ToString());
                TurnOrder();
                enemySpared = false;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                enemies[enemyIndex].SetActive(false);
                Debug.Log("test e");
                yield return null;
                state = BattleState.Win;
                enemySpared = false;
                yield return (BattleWin());
            }
        }
        else
        {
            if (enemyCaptured)
            {
                enemyCaptured = false;

                if (numEnemies > 0)
                {
                    enemies[enemyIndex].SetActive(false);
                    RandomizeEnemy();
                    SetSkills(ref playerCreature, true);//set player skills after enemy is randomized to the skills target properly
                    combatMenuScript.SetEnemy();
                    SetupEnemy();
                    SetSkills(ref enemyCreature, false);

                    yield return (DisplayingDialogue($"Enemy {enemyCreature.name} appears!"));
                    yield return new WaitForSeconds(textStop);

                    numEnemies -= 1;
                    Debug.Log("Enemies remaining: " + numEnemies.ToString());
                    TurnOrder();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    enemies[enemyIndex].SetActive(false);
                    yield return null;
                    state = BattleState.Win;
                    yield return (BattleWin());
                }
            }
            else
            {
                if ((enemyCreature.health / enemyCreature.MaxHealth) < 0f)
                {
                    SetToSpare(ref combatMenuScript.fleeButton);
                }
                else
                {
                    SetToFlee(ref combatMenuScript.fleeButton);
                }

                //checks if enemy was defeted
                if (enemyCreature.health <= 0)
                {
                    yield return (DisplayingDialogue($"Enemy {enemyCreature.name} was defeated"));
                    yield return new WaitForSeconds(textStop);
                    /*if (playerCreature.UpdateEXP(-(playerCreature.expCalc(enemyCreature.baseExpYield, enemyCreature.level))))
                    {
                        Skill temp = playerCreature.GetPotentialSkill();
                        if (temp.name != "")
                        {
                            skillMgr.CheckSkills(playerCreature, temp);
                        }
                    }*/
                    int exp = (playerCreature.expCalc(enemyCreature.baseExpYield, enemyCreature.level));
                    yield return (DisplayingDialogue($"{playerCreature.name} gains {exp} exp"));
                    //Debug.Log("After exp gain");
                    yield return new WaitForSeconds(textStop);
                    //Debug.Log("After first wait");
                    yield return null;
                    //Debug.Log("After second wait");

                    if (playerCreature.UpdateEXP(-exp))
                    {
                        //playerCreature.LearnSkill();
                    }
                    Debug.Log("After exp");

                    if (numEnemies > 1)
                    {
                        enemies[enemyIndex].SetActive(false);
                        RandomizeEnemy();
                        SetSkills(ref playerCreature, true);//set player skills after enemy is randomized to the skills target properly
                        combatMenuScript.SetEnemy();
                        SetupEnemy();

                        yield return (DisplayingDialogue($"Enemy {enemyCreature.name} appears!"));
                        //dialogueBox.text = "Enemy " + enemyCreature.name + " appears!";
                        yield return new WaitForSeconds(textStop);

                        numEnemies -= 1;
                        Debug.Log("Enemies remaining: " + numEnemies.ToString());
                        TurnOrder();
                        yield return new WaitForSeconds(1f);
                    }
                    else
                    {
                        enemies[enemyIndex].SetActive(false);
                        yield return null;
                        Debug.Log("before battleWin");
                        state = BattleState.Win;
                        yield return (BattleWin());
                    }
                }
                else
                {
                    Debug.Log("Enemy Turn");
                    yield return (DisplayingDialogue($"Enemy's turn"));
                    //dialogueBox.text = "Enemy's turn";

                    yield return new WaitForSeconds(textStop);

                    int enemyAction = 0;
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

                        yield return (DisplayingDialogue($"Enemy {enemyCreature.name} is stunned..."));
                        //dialogueBox.text = enemyCreature.name + " is stunned...";
                    }

                    bool temp = false;
                    if (intelegentEnemy)
                    {
                        for (int i = 0; i < skillButtons.Length; i++)
                        {
                            if ((enemyCreature.activeSkills[i].damageType == playerCreature.weakness) && (enemyCreature.activeSkills[i].power >= enemyCreature.activeSkills[enemyAction].power) && (enemyCreature.activeSkills[i].cost <= enemyCreature.energy))
                            {
                                enemyAction = i;
                                temp = true;
                            }
                        }
                        if (temp)
                        {
                            acted = combatMenuScript.UseSkill(enemyCreature.activeSkills[enemyAction]);
                        }
                    }

                    while (!acted)
                    {
                        enemyAction = Random.Range(0, skillButtons.Length + 2);

                        if (enemyCreature.isFrozen && enemyAction == enemyCreature.freezeIndex)
                        {
                            while (enemyAction == enemyCreature.freezeIndex)
                            {
                                enemyAction = Random.Range(0, skillButtons.Length + 2);
                            }
                        }

                        Debug.Log("Enemy Action" + enemyAction.ToString());
                        if (enemyAction >= enemyCreature.activeSkills.Count && enemyAction < skillButtons.Length)
                        {
                            enemyAction = 4;
                        }

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
                    Debug.Log($"{playerCreature.health}");
                    if (playerCreature.health <= 0)
                    {
                        numPlayerShades--;
                        if (numPlayerShades <= 0)
                        {
                            playerShades[playerIndex].SetActive(false);
                            yield return new WaitForSeconds(1f);
                            state = BattleState.Lose;
                            yield return (BattleLoss());
                        }
                        else
                        {
                            yield return (DisplayingDialogue($"{playerCreature.name} was defeated"));
                            partyMenu.SetActive(true);
                            combatMenuScript.OpenPartyMenu();
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(1f);
                        state = BattleState.PlayerTurn;
                        yield return (PlayerTurn());
                    }
                }
            }
        }
        yield return null;
    }

    public void partySaveTest()
    {
        for (int i = 0; i < playerShades.Count; i++)
        {
            Debug.Log($"{PartyData.party[i].GetComponent<Shade>().name}");
            Debug.Log($"{PartyData.party[i].GetComponent<Shade>().level}");
            Debug.Log($"{PartyData.party[i].GetComponent<Shade>().exp}");
            Debug.Log($"party size: {PartyData.partySize}");
        }
    }

    public IEnumerator BattleWin()
    {
        Debug.Log("start battleWin");
        yield return (DisplayingDialogue($"You've won the Battle!"));
        //dialogueBox.text = "Enemy " + enemyCreature.name + " was defeated";
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetInt("playerShadeIndex", playerIndex);
        Debug.Log("before save");
        SavePartyData();
        partySaveTest();
        Debug.Log("after save");
        EndBattle();
    }

    IEnumerator BattleLoss()
    {
        yield return (DisplayingDialogue($"You were defeated"));
        //dialogueBox.text = "You were defeated";
        yield return new WaitForSeconds(3f);
        SavePartyData();
        partySaveTest();
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
        //SceneManager.LoadScene(lastScene);
    }

    public void RandomizeEnemy()
    {
        do
        {
            Debug.Log("enemies.Length: " + (enemies.Count).ToString());
            enemyIndex = Random.Range(0, enemies.Count);
        }
        while (enemyIndex == previousEnemy);
        previousEnemy = enemyIndex;
        Debug.Log("Enemy index: " + enemyIndex.ToString());
        SetShade(ref enemy, enemies, enemyIndex, ref enemyCreature);

        enemyCreature.level = (Random.Range(enemyLevelMin, enemyLevelMax + 1));
        //enemyCreature.level = 10;

        SetSkills(ref enemyCreature, false);
    }

    public void SelectEnemy()
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

            enemyCreature.level = (Random.Range(enemyLevelMin, enemyLevelMax + 1));
            //enemyCreature.level = 10;
            SetSkills(ref enemyCreature, false);
        }
    }

    public void SetShade(ref GameObject shadeLocation, List<GameObject>shades, int shadeIndex, ref Shade activeShade)
    {   
        shadeLocation = shades[shadeIndex];
        shades[shadeIndex].SetActive(true);
        activeShade = shades[shadeIndex].GetComponent<Shade>();
    }

    public void SetSkills(ref Shade activeShade, bool isPlayer)
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            Skill temp = activeShade.activeSkills[i];
            SetupSkill(ref temp, isPlayer, activeShade);
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
        else//
        {
            skillButton.interactable = true;
            skillButton.onClick.RemoveAllListeners();
            skillButton.onClick.AddListener(delegate { skillMenu.SetActive(false); });
            skillButton.onClick.AddListener(delegate { combatMenuScript.UseSkill(activeShade.activeSkills[skillIndex]); });

            skillButton.onClick.AddListener(delegate { combatMenuScript.ChangeDescription(activeShade.activeSkills[skillIndex].description); });
            skillButton.onClick.AddListener(delegate { combatMenuScript.descriptionObject.SetActive(false); });
        }
    }

    public void SetToSpare(ref Button button)
    {
        sparePossible = true;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Spare";
        Color c = new Color(0.8823529f, 0.8823529f, 0.1960784f);
        button.GetComponentInChildren<TextMeshProUGUI>().color = c;
        //button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(delegate { combatMenuScript.SpareEnemy(); });
    }

    public void SetToFlee(ref Button button)
    {
        sparePossible = false;
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Flee";
        Color c = new Color(0.1960784f, 0.1960784f, 0.1960784f);
        button.GetComponentInChildren<TextMeshProUGUI>().color = c;
        //button.onClick.RemoveAllListeners();
        //button.onClick.AddListener(delegate { combatMenuScript.Flee(); });
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
        enemyCreature.SetupSkills();
        enemyCreature.SetupInitialEXP();
    }

    public void SavePartyData()
    {
        for (int i = 0; i < playerShades.Count; i++)
        {
            PartyData.party[i] = playerShades[i];
        }
        PartyData.partySize = playerShades.Count;
        PartyData.saved = true;
    }

   public void LoadPartyData()
    {
        for (int i = 0; i < PartyData.partySize; i++)
        {
            playerShades[i] = PartyData.party[i];
        }
    }

    /*
    public void SavePartyData(Shade[] playerParty)
    {
        for (int i = 0; i < playerShades.Count; i++)
        {
            playerParty[i] = playerShades[i].GetComponent<Shade>();
        }

        PartySaveMgr.SaveParty(playerParty);
    }

    public void LoadPartyData(Shade[] playerParty, List<GameObject> partyShades)
    {
        PartyData partyData = PartySaveMgr.LoadParty();

        for (int i = 0; i < partyData.level.Length; i++)
        {
            playerParty[i].name = partyData.name[i];
            playerParty[i].level = partyData.level[i];

            playerParty[i].baseMaxHealth = partyData.baseMaxHealth[i];
            playerParty[i].baseMaxEnergy = partyData.baseMaxEnergy[i];
            playerParty[i].baseAttack = partyData.baseAttack[i];
            playerParty[i].baseDefense = partyData.baseDefense[i];
            playerParty[i].baseSpeed = partyData.baseSpeed[i];
            playerParty[i].index = partyData.spriteIndex[i];
            playerParty[i].exp = partyData.exp[i];
            playerParty[i].lightLevels = partyData.lightLevels[i];
            playerParty[i].darkLevels = partyData.darkLevels[i];
            playerParty[i].levelExp = partyData.levelExp[i];
            playerParty[i].totalExp = partyData.totalExp[i];
            playerParty[i].requiredEXP = partyData.requiredEXP[i];
            playerParty[i].baseRequiredExp = partyData.baseRequiredExp[i];
            playerParty[i].weakness = ((DamageType)partyData.weakness[i]);

            for (int j = 0; j < partyData.skillNames[i].Length; j++)
            {
                playerParty[i].activeSkills[j].name = partyData.skillNames[i][j];
                playerParty[i].activeSkills[j].power = partyData.skillPowers[i][j];
                playerParty[i].activeSkills[j].cost = partyData.skillCosts[i][j];
                playerParty[i].activeSkills[j].damageType = ((DamageType)partyData.skillDamageTypes[i][j]);
                playerParty[i].activeSkills[j].isTargetSelf = partyData.skillIsTargetSelf[i][j];
                playerParty[i].activeSkills[j].effectTargetSelf = partyData.skillEffectTargetSelf[i][j];
                playerParty[i].activeSkills[j].effect = ((Effect)partyData.skillEffects[i][j]);
                playerParty[i].activeSkills[j].animationType = ((AnimationType)partyData.skillAnimationTypes[i][j]);
                playerParty[i].activeSkills[j].description = partyData.skillDescriptions[i][j];                
            }
        }

        for (int i = 0; i < party.Length; i++)
        {
            partyShades[i] = shades[playerParty[i].index];

            partyShades[i].GetComponent<Shade>().name = playerParty[i].name;
            partyShades[i].GetComponent<Shade>().level = playerParty[i].level;
            partyShades[i].GetComponent<Shade>().baseMaxHealth = playerParty[i].baseMaxHealth;
            partyShades[i].GetComponent<Shade>().baseMaxEnergy = playerParty[i].baseMaxEnergy;
            partyShades[i].GetComponent<Shade>().baseAttack = playerParty[i].baseAttack;
            partyShades[i].GetComponent<Shade>().baseDefense = playerParty[i].baseDefense;
            partyShades[i].GetComponent<Shade>().baseSpeed = playerParty[i].baseSpeed;
            partyShades[i].GetComponent<Shade>().index = playerParty[i].index;
            partyShades[i].GetComponent<Shade>().exp = playerParty[i].exp;
            partyShades[i].GetComponent<Shade>().lightLevels = playerParty[i].lightLevels;
            partyShades[i].GetComponent<Shade>().darkLevels = playerParty[i].darkLevels;
            partyShades[i].GetComponent<Shade>().levelExp = playerParty[i].levelExp;
            partyShades[i].GetComponent<Shade>().totalExp = playerParty[i].totalExp;
            partyShades[i].GetComponent<Shade>().requiredEXP = playerParty[i].requiredEXP;
            partyShades[i].GetComponent<Shade>().baseRequiredExp = playerParty[i].baseRequiredExp;
            partyShades[i].GetComponent<Shade>().weakness = playerParty[i].weakness;
            for (int j = 0; j < playerParty[i].activeSkills.Count; j++)
            {
                partyShades[i].GetComponent<Shade>().activeSkills.Add(playerParty[i].activeSkills[j]);
            }
        }
    }
    */

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

    public IEnumerator DisplayingDialogue(string text)
    {        
        dialogueBox.text = text;
        yield return new WaitForSeconds(1f);

        /*foreach (var letter in text.ToCharArray())
        {
            dialogueBox.text += letter;

            yield return new WaitForSeconds(1f / battleTextSpeed);
        }*/
    }
}
