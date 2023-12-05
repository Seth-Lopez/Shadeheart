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

    void Start()
    {
        // Register the buttons with their respective methods
        /*
        attackButton.onClick.AddListener(Attack);
        defendButton.onClick.AddListener(Defend);
        useItemButton.onClick.AddListener(UseItem);
        fleeButton.onClick.AddListener(Flee);
        */
        enemyCreature = battle.enemies[battle.enemyShade].GetComponent<Shade>();
    }

    public void Attack()
    {
        Debug.Log("Attack selected.");
        // Add your attack logic here

        StartCoroutine(PlayerAttack());
    }
    IEnumerator PlayerAttack()
    {
        playerCreature.defending = false;

        if (playerCreature.energy < 5)
        {
            dialougeBox.text = playerCreature.name + " doesn't have enough energy.";
            yield return new WaitForSeconds(2f);
            combatMenu.SetActive(true);
            dialougeBox.text = "Player's turn";
        }

        else
        {
            dialougeBox.text = playerCreature.name + " attacks!";

            int damage = 0;
            if (playerCreature.charged)
            {
                damage = Random.Range(12, 21);
                playerCreature.charged = false;
            }
            else
            {
                damage = Random.Range(8, 14);
                if (enemyCreature.defending)
                {
                    damage /= 2;
                }
            }
            Debug.Log(damage.ToString());
            enemyCreature.UpdateHealth(damage);
            playerCreature.UpdateEnergy(5);
            yield return new WaitForSeconds(1f);
            actionMenu.SetActive(false);
            combatMenu.SetActive(true);

            battle.StartEnemyTurn();
        }
    }

    public void EnemyAttack()
    {
        Debug.Log("Enemy Attacks");

        enemyCreature.defending = false;

        dialougeBox.text = enemyCreature.name + " attacks!";

        int damage = 0;
        if (enemyCreature.charged)
        {
            damage = Random.Range(12, 21);
            enemyCreature.charged = false;
        }
        else
        {
            damage = Random.Range(8, 14);
            if (playerCreature.defending)
            {
                damage /= 2;
            }
        }

        playerCreature.UpdateHealth(damage);
        enemyCreature.UpdateEnergy(5);
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

        playerCreature.defending = true;

        playerCreature.UpdateEnergy(-3);
        yield return new WaitForSeconds(1f);
        actionMenu.SetActive(false);
        combatMenu.SetActive(true);

        battle.StartEnemyTurn();
    }

    public void EnemyDefend()
    {
        Debug.Log("Enemy Defends.");

        dialougeBox.text = enemyCreature.name + " guards itself";

        enemyCreature.defending = true;
        playerCreature.UpdateEnergy(-3);
    }

    public void UseItem()
    {
        Debug.Log("Use Item selected.");
        // Add your item use logic here

        StartCoroutine(Item());
    }
    IEnumerator Item()
    {
        playerCreature.defending = false;

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

        playerCreature.defending = false;

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
            yield return null;
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
        playerCreature.defending = false;

        if (playerCreature.energy < 10)
        {
            dialougeBox.text = playerCreature.name + " doesn't have enough energy.";
            yield return new WaitForSeconds(2f);
            combatMenu.SetActive(true);
            dialougeBox.text = "Player's turn";
        }
        else
        {
            dialougeBox.text = playerCreature.name + " is charging its power";
            playerCreature.charged = true;
            playerCreature.UpdateEnergy(10);
            yield return new WaitForSeconds(1f);

            actionMenu.SetActive(false);
            combatMenu.SetActive(true);

            battle.StartEnemyTurn();
        }
    }

    public void EnemyCharge()
    {
        Debug.Log("Enemy Charging");

        enemyCreature.defending = false;

        dialougeBox.text = enemyCreature.name + " is charging its power";
        enemyCreature.charged = true;
        enemyCreature.UpdateEnergy(10);
    }
}
