using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillMgr : MonoBehaviour
{
    public TextMeshProUGUI levelDialouge, skillDescription, skillPower, skillCost;
    public GameObject learnSkillMenu, skillDescriptionObject;
    public BattleMgr battle;
    public Button[] learnSkillButtons;
    public GameObject[] skillButtonObjects;
    private int index = -1;
    private Shade currentShade;
    public Skill potentialSkill;
    private bool selected = false;

    public void Update()
    {
        for (int i = 0; i <= 4; i++)
        {
            if (i == 4 && EventSystem.current.currentSelectedGameObject == skillButtonObjects[i])
            {
                ChangeDescription(potentialSkill.description);
                //Debug.Log("Changed Description");

                skillPower.text = "Power: " + Mathf.Abs(potentialSkill.power).ToString();
                skillCost.text = "Cost: " + potentialSkill.cost.ToString();

                break;
            }
            else if (EventSystem.current.currentSelectedGameObject == skillButtonObjects[i])
            {
                ChangeDescription(currentShade.activeSkills[i].description);
                //Debug.Log("Changed Description");

                skillPower.text = "Power: " + Mathf.Abs(currentShade.activeSkills[i].power).ToString();
                skillCost.text = "Cost: " + currentShade.activeSkills[i].cost.ToString();

                break;
            }
            else
            {
                ChangeDescription("");
                skillPower.text = "";
                skillCost.text = "";
            }
        }
    }

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public void SetSelected(bool newSelected)
    {
        selected = newSelected;
    }

    public void SetButtons(ref Shade shade, Skill skill)
    {
        learnSkillMenu.SetActive(true);

        for (int i = 0; i < learnSkillButtons.Length; i++)
        {
            SetupButton(ref learnSkillButtons[i], i, skill, shade);
        }
    }

    public void SetupButton(ref Button button, int skillIndex, Skill skill, Shade shade)
    {
        Debug.Log("Setup button");
        if (skillIndex == learnSkillButtons.Length-1)
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = skill.name;
        }
        else
        {
            button.GetComponentInChildren<TextMeshProUGUI>().text = shade.activeSkills[skillIndex].name;
        }
        if (button.GetComponentInChildren<TextMeshProUGUI>().text == "")
        {
            button.interactable = false;
        }
        else//
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { learnSkillMenu.SetActive(false); });
            button.onClick.AddListener(delegate { SetIndex(skillIndex); });

            button.onClick.AddListener(delegate { SetSelected(true); });

            button.onClick.AddListener(delegate { skillDescriptionObject.SetActive(false); });
        }
    }

    public IEnumerable SkillChoice(Shade shade, Skill skill)
    {
        potentialSkill = skill;
        yield return (DisplayDialogue($"{name} can learn {skill.name}.\nWhich skill should be replaced?"));
        SetButtons(ref shade, skill);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(skillButtonObjects[1]);
    }

    public void CheckSkills(Shade shade, Skill skill)
    {
        if (shade.activeSkills.Count >= 4)
        {
           SkillChoice(shade, skill);
        }//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //shade.LearnSkill(skill, index);
        SetIndex(-1);
        SetSelected(false);
    }

    public IEnumerator DisplayDialogue(string text)
    {
        yield return new WaitForSeconds(1f);
        levelDialouge.text = "";

        foreach (var letter in text.ToCharArray())
        {
            levelDialouge.text += letter;

            yield return new WaitForSeconds(1f / 30);
        }
    }

    public void ChangeDescription(string description)
    {
        skillDescription.text = description;
    }

    public void LearnSkill(Shade shade)
    {
        if (shade.exp >= 0)
        {
            foreach (var skill in shade.lightSkills)
            {
                if (skill.Level <= shade.lightLevels && skill.learned == false)
                {
                    skill.learned = true;

                    if (shade.activeSkills.Count >= 4)
                    {
                        //ask player which skill to remove
                        shade.activeSkills.RemoveAt(0);
                    }
                    shade.activeSkills.Add(skill.BaseSkill);
                }
            }
        }
        else
        {
            foreach (var skill in shade.darkSkills)
            {
                if (skill.Level <= shade.darkLevels && skill.learned == false)
                {
                    skill.learned = true;

                    if (shade.activeSkills.Count >= 4)
                    {
                        //ask player which skill to remove
                        shade.activeSkills.RemoveAt(0);
                    }
                    shade.activeSkills.Add(skill.BaseSkill);
                }
            }
        }
    }
}
