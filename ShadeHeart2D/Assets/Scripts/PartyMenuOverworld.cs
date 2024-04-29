using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;

public class PartyMenuOverworld : MonoBehaviour
{
    Shade[] playerShades;
    GameObject[] party;
    int activeIndex;
    public GameObject[] shades;

    [SerializeField] GameObject[] partyImages;
    [SerializeField] GameObject[] partyMenuHUDs;
    //[SerializeField] GameObject[] partyButtonObjects;
    [SerializeField] Button[] partyButtons;

    [SerializeField] Meter[] healthMeters;
    [SerializeField] Meter[] energyMeters;

    [SerializeField] TextMeshProUGUI[] names;

    [SerializeField] GameObject[] activeIcon; //icons that appear if a the shade is selected

    public GameObject partyMenu;

    // Start is called before the first frame update
    void Start()
    {
        activeIndex = PlayerPrefs.GetInt("playerShadeIndex");
        string saveDataPath = Application.persistentDataPath + "/party.sav";
        Debug.Log(saveDataPath);
        /*if (File.Exists(saveDataPath))
        {
            LoadPartyData(playerShades, party);

            for (int i = 0; i < partyMenuHUDs.Length; i++)
            {
                partyButtons[i].interactable = false;
                partyMenuHUDs[i].SetActive(false);
                activeIcon[i].SetActive(false);
            }

            activeIcon[activeIndex].SetActive(true);

            for (int i = 0; i < party.Length; i++)
            {
                partyButtons[i].interactable = true;
                partyMenuHUDs[i].SetActive(true);
                partyImages[i].GetComponent<Image>().sprite = party[i].GetComponent<SpriteRenderer>().sprite;
                names[i].text = party[i].name;
                healthMeters[i].SetMaxValueMenu(party[i].GetComponent<Shade>().MaxHealth);
                energyMeters[i].SetMaxValueMenu(party[i].GetComponent<Shade>().MaxEnergy);

                /*for (int j = 0; j < battle.playerShades[i].GetComponent<Shade>().activeSkills.Count; j++)
                {
                    if (battle.playerShades[i].GetComponent<Shade>().activeSkills[j].damageType == battle.enemyCreature.weakness)
                    {
                        activeIcon[i].SetActive(true);
                        break;
                    }
                }
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPartyMenu()
    {
        //sets currently active shade's button to selected
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(party[activeIndex]);

        for (int i = 0; i < party.Length; i++)
        {
            healthMeters[i].SetValueMenu(party[i].GetComponent<Shade>().health);
            energyMeters[i].SetValueMenu(party[i].GetComponent<Shade>().energy);
        }
    }

    public void SwitchActive(int newIndex)
    {
        if (party[newIndex].GetComponent<Shade>().health <= 0)
        {
            //StartCoroutine(battle.DisplayingDialogue($"Unable to switch.\n{battle.playerShades[newIndex].GetComponent<Shade>().name} is KOd..."));
            partyMenu.SetActive(true);
            OpenPartyMenu();
        }
        else
        {
            activeIcon[activeIndex].SetActive(false);
            activeIcon[newIndex].SetActive(true);
            activeIndex = newIndex;
            PlayerPrefs.SetInt("playerShadeIndex", newIndex);
        }
    }
    /*
    public void SavePartyData(Shade[] playerParty)
    {
        for (int i = 0; i < party.Length; i++)
        {
            playerParty[i] = party[i].GetComponent<Shade>();
        }

        PartySaveMgr.SaveParty(playerParty);
    }

    public void LoadPartyData(Shade[] playerParty, GameObject[] partyShades)
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
}
