using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    [SerializeField] GameObject[] party;

    [SerializeField] GameObject[] partyImages;
    [SerializeField] GameObject[] partyMenuHUDs;

    [SerializeField] Button[] partyButtons;


    [SerializeField] Meter[] healthMeters;
    [SerializeField] Meter[] energyMeters;

    [SerializeField] TextMeshProUGUI[] names;

    [SerializeField] GameObject[] suggested; //icons that appear if a shade has a skill that the current enemy is vulnerable to

    [SerializeField] BattleMgr battle;
    [SerializeField] CombatMenu combatMenuScript;

    [SerializeField] int activeIndex;
    // Start is called before the first frame update
    void Start()
    {
        activeIndex = PlayerPrefs.GetInt("playerShadeIndex");
        //Debug.Log("Test1");

        for (int i = 0; i < partyMenuHUDs.Length; i++)
        {
            //Debug.Log("Test A-"+i.ToString());
            partyButtons[i].interactable = false;
            //Debug.Log("inteactable " + i.ToString() + ": " + partyButtons[i].interactable.ToString());
            //Debug.Log("PartyMenuHUDs Length: " + partyMenuHUDs.Length.ToString());

            partyMenuHUDs[i].SetActive(false);

            suggested[i].SetActive(false);
        }
        //Debug.Log("Test2");

        for (int i = 0; i < battle.playerShades.Length; i++)
        {
            //Debug.Log("inteactable " + i.ToString() + ": " + partyButtons[i].interactable.ToString());
            partyButtons[i].interactable = true;
            partyMenuHUDs[i].SetActive(true);
            partyImages[i].GetComponent<Image>().sprite = battle.playerShades[i].GetComponent<SpriteRenderer>().sprite;
            names[i].text = battle.playerShades[i].name;
            healthMeters[i].SetMaxValueMenu(battle.playerShades[i].GetComponent<Shade>().MaxHealth);
            energyMeters[i].SetMaxValueMenu(battle.playerShades[i].GetComponent<Shade>().MaxEnergy);

            for (int j = 0; j < battle.playerShades[i].GetComponent<Shade>().activeSkills.Length; j++)
            {
                if (battle.playerShades[i].GetComponent<Shade>().activeSkills[j].damageType == battle.enemyCreature.weakness)
                {
                    suggested[i].SetActive(true);
                    break;
                }
            }
        }
        //Debug.Log("Test3");
    }

    public void OpenPartyMenu()
    {
        //sets currently active shade's button to selected
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(party[activeIndex]);

        for (int i = 0; i < battle.playerShades.Length; i++)
        {
            healthMeters[i].SetValueMenu(battle.playerShades[i].GetComponent<Shade>().health);
            energyMeters[i].SetValueMenu(battle.playerShades[i].GetComponent<Shade>().energy);
        }
    }

    public void SwitchActive(int newIndex)
    {
        //turn off active shade
        battle.playerShades[activeIndex].SetActive(false);
        //set activeIndex to newIndex
        activeIndex = newIndex;
        battle.playerIndex = newIndex;
        //set new active shade
        battle.SetShade(ref battle.player, battle.playerShades, newIndex, ref battle.playerCreature);
        combatMenuScript.SetPlayer();
        battle.SetupPlayer();
        //call SetSkills function so skills target the correct shades
        battle.SetSkills(ref battle.enemyCreature, false);
        battle.SetSkills(ref battle.playerCreature, true);
        battle.TurnOrder();
    }
}
