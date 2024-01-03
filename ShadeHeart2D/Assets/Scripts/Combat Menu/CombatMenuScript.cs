using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CombatMenu : MonoBehaviour
{
    // Buttons on the UI
    public Button actionButton;
    public Button useItemButton;
    public Button fleeButton;
    public Button attackButton;
    public Button defendButton;
    public Button chargeButton;

    public BattleMgr battle;
    public TextMeshProUGUI dialougeBox;
    public Shade playerCreature, enemyCreature;

    public GameObject combatMenu;
    public GameObject actionMenu;

    float basicAtkPower = 25;
    float randDamageMin = 80;
    float randDamageMax = 100;
    public int basicAttackCost, defendCost, chargeCost;

    public Animator playerAttackingAnim, enemyAttackingAnim;

    void Start()
    {
        // Register the buttons with their respective methods
        /*
        attackButton.onClick.AddListener(Attack);
        defendButton.onClick.AddListener(Defend);
        useItemButton.onClick.AddListener(UseItem);
        fleeButton.onClick.AddListener(Flee);
        */
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
            actionMenu.SetActive(false);
            combatMenu.SetActive(true);

            playerAttackingAnim.SetBool("isAttacking", false);

            battle.StartEnemyTurn();
        }
    }

    public void EnemyAttack()
    {
        Debug.Log("Enemy Attacks");

        enemyCreature.isDefending = false;

        dialougeBox.text = enemyCreature.name + " attacks!";

        enemyAttackingAnim.SetBool("isAttacking", true);

        float damage = DamageCalc(enemyCreature, playerCreature, basicAtkPower, enemyCreature.basicAttackType);
        Debug.Log("Enemy Damage: " + damage.ToString());

        playerCreature.UpdateHealth(damage);
        enemyCreature.UpdateEnergy(basicAttackCost);

        playerAttackingAnim.SetBool("isAttacking", false);
    }

    public void Defend()
    {
        Debug.Log("Defend selected.");
        // Add your defend logic here

        StartCoroutine(PlayerDefend());
    }
    IEnumerator PlayerDefend()
    {
        dialougeBox.text = playerCreature.name + " guards itself";

        playerCreature.isDefending = true;

        playerCreature.UpdateEnergy(defendCost);
        yield return new WaitForSeconds(1f);
        actionMenu.SetActive(false);
        combatMenu.SetActive(true);

        battle.StartEnemyTurn();
    }

    public void EnemyDefend()
    {
        Debug.Log("Enemy Defends.");

        dialougeBox.text = enemyCreature.name + " guards itself";

        enemyCreature.isDefending = true;
        playerCreature.UpdateEnergy(defendCost);
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

    public void Charge()
    {
        Debug.Log("Charge selected.");
        StartCoroutine(PlayerCharge());
    }
    IEnumerator PlayerCharge()
    {
        playerCreature.isDefending = false;

        if (playerCreature.energy < chargeCost)
        {
            dialougeBox.text = playerCreature.name + " doesn't have enough energy.";
            yield return new WaitForSeconds(2f);
            combatMenu.SetActive(true);
            battle.OpenCombatMenu();
            dialougeBox.text = "Player's turn";
        }
        else
        {
            dialougeBox.text = playerCreature.name + " is charging its power";
            playerCreature.charged = true;
            playerCreature.UpdateEnergy(chargeCost);
            yield return new WaitForSeconds(1f);

            actionMenu.SetActive(false);
            combatMenu.SetActive(true);

            battle.StartEnemyTurn();
        }
    }

    public void EnemyCharge()
    {
        Debug.Log("Enemy Charging");

        enemyCreature.isDefending = false;

        dialougeBox.text = enemyCreature.name + " is charging its power";
        enemyCreature.charged = true;
        enemyCreature.UpdateEnergy(chargeCost);
    }

    public float DamageCalc(Shade attackingCreature, Shade defendingCreature, float power, DamageType damageType)
    {
        float damage = ((Random.Range(randDamageMin, randDamageMax)/100) * ((power / 100) * attackingCreature.attack / ((defendingCreature.defense) / 100)));
        if (attackingCreature.charged)
        {
            damage += damage/2;
            attackingCreature.charged = false;
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

    public void SetEnemy()
    {
        enemyCreature = battle.enemies[battle.enemyIndex].GetComponent<Shade>();
    }
}
