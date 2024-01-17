using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PartyMenu : MonoBehaviour
{
    public GameObject[] party;

    public GameObject[] partyImages;
    public GameObject[] partyMenuHUDs;

    public Button[] partyButtons;


    public Meter[] healthMeters;
    public Meter[] energyMeters;

    public TextMeshProUGUI[] names;

    public BattleMgr battle;
    public CombatMenu combatMenuScript;

    public int activeIndex;
    // Start is called before the first frame update
    void Start()
    {
        activeIndex = PlayerPrefs.GetInt("playerShadeIndex");
        Debug.Log("Test1");

        for (int i = 0; i < partyMenuHUDs.Length; i++)
        {
            Debug.Log("Test A-"+i.ToString());
            partyButtons[i].interactable = false;
            Debug.Log("inteactable " + i.ToString() + ": " + partyButtons[i].interactable.ToString());
            Debug.Log("PartyMenuHUDs Length: " + partyMenuHUDs.Length.ToString());

            partyMenuHUDs[i].SetActive(false);
        }
        Debug.Log("Test2");

        for (int i = 0; i < battle.playerShades.Length; i++)
        {
            Debug.Log("inteactable " + i.ToString() + ": " + partyButtons[i].interactable.ToString());
            partyButtons[i].interactable = true;
            partyMenuHUDs[i].SetActive(true);
            partyImages[i].GetComponent<Image>().sprite = battle.playerShades[i].GetComponent<SpriteRenderer>().sprite;
            names[i].text = battle.playerShades[i].name;
            healthMeters[i].SetMaxValueMenu(battle.playerShades[i].GetComponent<Shade>().maxHealth);
            energyMeters[i].SetMaxValueMenu(battle.playerShades[i].GetComponent<Shade>().maxEnergy);
        }
        Debug.Log("Test3");
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
    }
}
