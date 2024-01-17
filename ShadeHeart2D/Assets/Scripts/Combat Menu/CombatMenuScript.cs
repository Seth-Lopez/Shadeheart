using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CombatMenu : MonoBehaviour
{
    // Buttons on the UI
    //public Button actionButton;
    public Button useItemButton;
    public Button fleeButton;
    public Button attackButton;
    public Button defendButton;
    public Button chargeButton;

    public BattleMgr battle;
    public TextMeshProUGUI dialougeBox;
    public Shade playerCreature, enemyCreature;

    public GameObject combatMenu;
    //public GameObject actionMenu;
    public GameObject skillMenu;
    public GameObject partyMenuMgr;

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
            dialougeBox.text = playerCreature.name + " doesn't have enough energy.";
            yield return new WaitForSeconds(2f);
            combatMenu.SetActive(true);
            battle.OpenCombatMenu();
            dialougeBox.text = "Player's turn";
        }

        else
        {
            dialougeBox.text = playerCreature.name + " attacks!";

            playerAttackingAnim.SetBool("isAttacking", true);

            float damage = DamageCalc(playerCreature, enemyCreature, basicAtkPower, playerCreature.basicAttackType);
            Debug.Log("Damage: " + damage.ToString());
            enemyCreature.UpdateHealth(damage);
            playerCreature.UpdateEnergy(basicAttackCost);

            yield return new WaitForSeconds(1f);

            playerAttackingAnim.SetBool("isAttacking", false);

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

        dialougeBox.text = enemyCreature.name + " attacks!";

        enemyAttackingAnim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1f);

        float damage = DamageCalc(enemyCreature, playerCreature, basicAtkPower, enemyCreature.basicAttackType);
        Debug.Log("Enemy Damage: " + damage.ToString());

        playerCreature.UpdateHealth(damage);
        enemyCreature.UpdateEnergy(basicAttackCost);

        playerAttackingAnim.SetBool("isAttacking", false);
        yield return null;
    }

    public void OpenPartyMenu()
    {
        Debug.Log("Open Party Menu");

        battle.OpenParty();
        //StartCoroutine(Party());
    }

    IEnumerator Party()
    {
        playerCreature.isDefending = false;

        dialougeBox.text = "No party...";

        yield return new WaitForSeconds(1f);

        dialougeBox.text = "Player's turn";

        yield return null;
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

        dialougeBox.text = "No items to use...";

        yield return new WaitForSeconds(1f);

        dialougeBox.text = "Player's turn";
        yield return null;
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
            dialougeBox.text = "You failed to escape!";
            yield return new WaitForSeconds(1f);
            battle.StartEnemyTurn();
        }
        else
        {
            dialougeBox.text = "You escaped!";
            yield return new WaitForSeconds(1f);
            battle.EndBattle();
        }
    }

    public float DamageCalc(Shade attackingCreature, Shade defendingCreature, float power, DamageType damageType)
    {
        float damage = ((Random.Range(randDamageMin, randDamageMax)/100) * ((power / 100) * attackingCreature.attack / ((defendingCreature.defense) / 100)));
        if (attackingCreature.isCharged)
        {
            damage += damage/2;
            attackingCreature.isCharged = false;
        }
        else if (defendingCreature.isDefending)
        {
            damage /= 2;
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
                dialougeBox.text = skill.user.ToString() + "doesn't have enough energy...";
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
            skill.target.UpdateHealth(damage);
            skill.user.UpdateEnergy(skill.cost);

            switch (skill.effect)
            {
                case Effect.Shock:
                case Effect.Burn:
                case Effect.Blind:
                case Effect.Freeze:
                    break;
                case Effect.Charge:
                    skill.effectTarget.isCharged = true;
                    break;
                case Effect.Defend:
                    skill.effectTarget.isDefending = true;
                    break;
                case Effect.None:
                default:
                    break;
            }

            dialougeBox.text = skill.user.name.ToString() + " used " + skill.name.ToString();

            StartCoroutine(Animate(skill.animationType, animator));

            if (battle.state == BattleState.PlayerTurn)
            {
                battle.StartEnemyTurn();
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