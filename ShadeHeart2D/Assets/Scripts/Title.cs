using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    string sceneName = "Battle";

    public int battleLocation = 0;
    public void StartGame()
    {
        sceneName = "OverWorld";
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    public void StartBattle()
    {
        sceneName = "Battle";
        PlayerPrefs.SetInt("battleLocation", battleLocation);
        int sceneLoadedFrom = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("sceneLoadedFrom", sceneLoadedFrom);
        SceneManager.LoadScene(sceneName);
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
}
