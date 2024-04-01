using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

public class CombatMenu : MonoBehaviour
{
    // Buttons on the UI
    //public Button actionButton;
    [SerializeField] private Button useItemButton;
    [SerializeField] Button fleeButton;
    [SerializeField] Button attackButton;
    [SerializeField] Button defendButton;
    [SerializeField] Button chargeButton;

    [SerializeField] BattleMgr battle;
    [SerializeField] TextMeshProUGUI dialogueBox;
    public Shade playerCreature, enemyCreature;
    [SerializeField] TextMeshProUGUI descriptionBox;
    public GameObject descriptionObject;
    [SerializeField] GameObject[] skillButtonObjects;
    [SerializeField] GameObject vulnerable;
    [SerializeField] TextMeshProUGUI skill_power;
    [SerializeField] TextMeshProUGUI skill_cost;

    public GameObject combatMenu;
    //[SerializeField] GameObject actionMenu;
    [SerializeField] GameObject skillMenu;
    [SerializeField] GameObject partyMenuMgr;

    float basicAtkPower = 20;
    float randDamageMin = 80;
    float randDamageMax = 100;
    public int basicAttackCost;

    public Animator playerAttackingAnim, enemyAttackingAnim, playerSelfAnim, enemySelfAnim;

    void Start()
    {
        // Register the buttons with their respective methods
        /*
        attackButton.onClick.AddListener(Attack);
        defendButton.onClick.AddListener(Defend);
        useItemButton.onClick.AddListener(UseItem);
        fleeButton.onClick.AddListener(Flee);
        */
        partyMenuMgr.SetActive(true);
        descriptionObject.SetActive(false);
    }
    
    private void Update()
    {
        for(int i = 0; i < playerCreature.activeSkills.Length; i++)
        {
            //Debug.Log(playerCreature.activeSkills.Length.ToString());
            //Debug.Log(EventSystem.current.currentSelectedGameObject);
            if (EventSystem.current.currentSelectedGameObject == skillButtonObjects[i])
            {
                ChangeDescription(playerCreature.activeSkills[i].description);
                //Debug.Log("Changed Description");

                skill_power.text = "Power: " + Mathf.Abs(playerCreature.activeSkills[i].power).ToString();
                skill_cost.text = "Cost: " + playerCreature.activeSkills[i].cost.ToString();

                if (playerCreature.activeSkills[i].damageType == enemyCreature.weakness)
                {
                    vulnerable.SetActive(true);
                }
                else
                {
                    vulnerable.SetActive(false);
                }
                break;
            }
            else
            {
                ChangeDescription("");
                skill_power.text = "";
                skill_cost.text = "";
            }
        }
    }
    

    public void ChangeDescription(string description)
    {
        descriptionBox.text = description;
    }

    public void Attack()
    {
        Debug.Log("Attack selected.");
        // Add your attack logic here

        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerAttack()
    {
        playerCreature.isDefending = false;

        if (playerCreature.energy < basicAttackCost)
        {
            yield return (battle.DisplayingDialogue($"{playerCreature.name} doesn't have enough energy."));
            //dialogueBox.text = playerCreature.name + " doesn't have enough energy.";
            yield return new WaitForSeconds(battle.textStop);
            combatMenu.SetActive(true);
            battle.OpenCombatMenu();

            yield return (battle.DisplayingDialogue($"Player's turn"));
            //dialogueBox.text = "Player's turn";
        }

        else
        {
            yield return (battle.DisplayingDialogue($"{playerCreature.name} attacks!"));
            //dialogueBox.text = playerCreature.name + " attacks!";

            playerAttackingAnim.SetBool("isAttacking", true);

            float damage = DamageCalc(playerCreature, enemyCreature, basicAtkPower, playerCreature.basicAttackType);
            Debug.Log("Damage: " + damage.ToString());
            enemyCreature.UpdateHealth(damage);
            playerCreature.UpdateEnergy(basicAttackCost);

            yield return new WaitForSeconds(1f);

            playerAttackingAnim.SetBool("isAttacking", false);

            if (playerCreature.isFrozen)
            {
                playerCreature.isFrozen = false;
                battle.skillButtons[playerCreature.freezeIndex].interactable = true;
            }

            battle.StartEnemyTurn();
        }
    }

    public void EnemyAttack()
    {
        StartCoroutine(EnemyAttacking());
    }

    IEnumerator EnemyAttacking()
    {
        Debug.Log("Enemy Attacks");

        enemyCreature.isDefending = false;

        yield return (battle.DisplayingDialogue($"{enemyCreature.name} attacks!"));
        //dialogueBox.text = enemyCreature.name + " attacks!";

        enemyAttackingAnim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);

        float damage = DamageCalc(enemyCreature, playerCreature, basicAtkPower, enemyCreature.basicAttackType);
        Debug.Log("Enemy Damage: " + damage.ToString());

        playerCreature.UpdateHealth(damage);
        enemyCreature.UpdateEnergy(basicAttackCost);

        if (enemyCreature.isFrozen)
        {
            enemyCreature.isFrozen = false;
        }

        playerAttackingAnim.SetBool("isAttacking", false);
        yield return null;
    }

    public void OpenPartyMenu()
    {
        Debug.Log("Open Party Menu");

        battle.OpenParty();
    }

    public void UseItem()
    {
        Debug.Log("Use Item selected.");
        // Add your item use logic here

        StartCoroutine(Item());
    }
    IEnumerator Item()
    {
        playerCreature.isDefending = false;

        yield return (battle.DisplayingDialogue($"No items to use..."));
        //dialogueBox.text = "No items to use...";

        yield return new WaitForSeconds(battle.textStop);

        yield return (battle.DisplayingDialogue($"Player's turn"));
        //dialogueBox.text = "Player's turn";
        //yield return null;
    }

    public void Flee()
    {
        StartCoroutine(Escape());
    }
    IEnumerator Escape()
    {
        Debug.Log("Flee selected.");
        // Add your flee logic here

        playerCreature.isDefending = false;

        int fleeResult = Random.Range(1, 21);
        Debug.Log(fleeResult.ToString());
        if (fleeResult <= 4)
        {
            yield return (battle.DisplayingDialogue($"You failed to escape!"));
            //dialogueBox.text = "You failed to escape!";
            yield return new WaitForSeconds(battle.textStop);
            battle.StartEnemyTurn();
        }
        else
        {
            yield return (battle.DisplayingDialogue($"You escaped!"));
            //dialogueBox.text = "You escaped!";
            yield return new WaitForSeconds(1f);
            battle.EndBattle();
        }
        yield return null;
    }

    public float DamageCalc(Shade attackingCreature, Shade defendingCreature, float power, DamageType damageType)
    {
        float damage = ((Random.Range(randDamageMin, randDamageMax)/100) * ((power / 100) * attackingCreature.Attack / ((defendingCreature.Defense) / 100)));
        if (attackingCreature.isCharged)
        {
            damage += damage/2;
            attackingCreature.isCharged = false;
        }
        else if (defendingCreature.isDefending)
        {
            damage /= 2;
        }
        if (defendingCreature.isBurned && damageType == DamageType.Fire)
        {
            damage += damage / 4;
        }
        if (defendingCreature.weakness == damageType)
        {
            damage *= 2;
        }
        damage = Mathf.Round(damage);
        return damage;
    }

    public void SetPlayer()
    {
        playerCreature = battle.playerShades[battle.playerIndex].GetComponent<Shade>();
    }

    public void SetEnemy()
    {
        enemyCreature = battle.enemies[battle.enemyIndex].GetComponent<Shade>();
    }

    public bool UseSkill(Skill skill)
    {
        if (skill.name == "")
        {
            return false;
        }
        else if (skill.cost > skill.user.energy)
        {
            if (battle.state == BattleState.PlayerTurn)
            {
                StartCoroutine(battle.DisplayingDialogue($"{skill.user} doesn't have enough energy..."));
                //dialogueBox.text = skill.user.ToString() + "doesn't have enough energy...";
                battle.skillMenu.SetActive(true);
                battle.OpenSkillMenu();
            }

            return false;
        }
        else
        {
            Animator animator;
            if (battle.state == BattleState.PlayerTurn)
            {
                if (skill.isTargetSelf)
                {
                    animator = playerSelfAnim;
                }
                else
                {
                    animator = playerAttackingAnim;
                }
            }
            else
            {
                if (skill.isTargetSelf)
                {
                    animator = enemySelfAnim;
                }
                else
                {
                    animator = enemyAttackingAnim;
                }
            }

            skill.user.isDefending = false;

            float damage = DamageCalc(skill.user, skill.target, skill.power, skill.damageType);
            Debug.Log(damage.ToString());
            skill.user.UpdateEnergy(skill.cost);
            skill.target.UpdateHealth(damage);

            switch (skill.effect)
            {
                case Effect.Stun://skip an enemy turn
                    if (!skill.effectTarget.wasStunned)
                    {
                        skill.effectTarget.isStunned = true;
                    }
                    break;
                //case Effect.Shock:
                case Effect.Burn://additional damage on next fire attack
                    skill.effectTarget.isBurned = true;
                    break;
                case Effect.Freeze://restrict usage of an enemy's skill
                    skill.effectTarget.freezeIndex = Random.Range(0, battle.skillButtons.Length);
                    skill.effectTarget.isFrozen = true;
                    break;
                case Effect.Charge://ignore defend skill and makes the next skill do 1.5x damage
                    skill.effectTarget.isCharged = true;
                    break;
                case Effect.Defend://reduces the damage from the next enemy attack to 0.5x
                    skill.effectTarget.isDefending = true;
                    break;
                case Effect.None:
                default:
                    break;
            }

            StartCoroutine(battle.DisplayingDialogue($"{skill.user.name} used {skill.name}"));
            //dialogueBox.text = skill.user.name.ToString() + " used " + skill.name.ToString();

            StartCoroutine(Animate(skill.animationType, animator));

            if (battle.state == BattleState.PlayerTurn)
            {
                battle.StartEnemyTurn();
            }

            if (skill.user.isFrozen)
            {
                skill.user.isFrozen = false;
                if (battle.state == BattleState.PlayerTurn)
                {
                    battle.skillButtons[skill.user.freezeIndex].interactable = true;
                }
            }

            return true;
        }
    }

    IEnumerator Animate(AnimationType animationType, Animator animator)
    {
        switch (animationType)
        {
            case AnimationType.None:
                yield return null;
                break;
            default:
                animator.SetBool(animationType.ToString(), true);
                yield return new WaitForSeconds(1f);
                animator.SetBool(animationType.ToString(), false);
                yield return null;
                break;
        }
    }
}