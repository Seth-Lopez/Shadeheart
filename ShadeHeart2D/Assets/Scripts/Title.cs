using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    string battleSceneName = "Battle";
    string overworldSceneName = "Seth_Testing";

    public int battleLocation = 0;

    public GameObject title, options, battleSelect;

    public SceneLoader loader;

    public GameObject titleOpenButton, optionsOpenButton, optionsCloseButton, locationOpenButton, locationCloseButton;

    public void Start()
    {
        title.SetActive(true);
        options.SetActive(false);
        battleSelect.SetActive(false);
        OpenTitleMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(overworldSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void StartBattle()
    {
        PlayerPrefs.SetInt("battleLocation", battleLocation);
        string sceneLoadedFrom = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("sceneLoadedFrom", sceneLoadedFrom);
        loader.LoadBattle(battleSceneName);
        //SceneManager.LoadScene(sceneName);
    }

    public void ForrestSelect()
    {
        battleLocation = 0;
    }

    public void DesertSelect()
    {
        battleLocation = 1;
    }

    public void SnowSelect()
    {
        battleLocation = 2;
    }

    public void NightSelect()
    {
        battleLocation = 3;
    }

    public void OpenTitleMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(titleOpenButton);
    }

    public void OpenOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsOpenButton);
    }

    public void ClsoeOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsCloseButton);
    }

    public void OpenLocationSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(locationOpenButton);
    }

    public void CloseLocationSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(locationCloseButton);
    }
}
