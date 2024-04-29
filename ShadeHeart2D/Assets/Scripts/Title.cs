using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] string battleSceneName = "Battle";
    [SerializeField] string overworldSceneName = "City";

    [SerializeField] int battleLocation = 0;

    public GameObject title, shadeSelect, options, battleSelect, loadingScreen;

    public SceneLoader loader;

    public GameObject titleOpenButton, selectOpenButton, selectCloseButton, optionsOpenButton, optionsCloseButton, locationOpenButton, locationCloseButton;

    public Slider loadingBar;

    public GameObject[] TitleBackgrounds;
    int backgroundIndex = 0;

    public void Start()
    {
        title.SetActive(true);
        shadeSelect.SetActive(false);
        options.SetActive(false);
        battleSelect.SetActive(false);
        loadingScreen.SetActive(false);
        OpenTitleMenu();
        backgroundIndex = Random.Range(0, 7);
        TitleBackgrounds[backgroundIndex].SetActive(true);
        StartCoroutine(ChangeBackground());
    }

    public void StartGame()
    {
        SceneManager.LoadScene(overworldSceneName);
    }

    public void StartLoadingGame()
    {
        StartCoroutine(LoadScene(overworldSceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        /*
        float loadingProgress = 0;

        while (!loading.isDone)
        {
            Debug.Log("test");
            loadingProgress += (Random.Range(70, 95)/100f);
            loadingBar.value = loadingProgress;
            Debug.Log(loadingProgress);
            

            yield return null;
        }*/

        yield return null;
    }

    IEnumerator ChangeBackground()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            TitleBackgrounds[backgroundIndex].SetActive(false);
            backgroundIndex++;
            if (backgroundIndex > 6)
            {
                backgroundIndex = 0;
            }
            TitleBackgrounds[backgroundIndex].SetActive(true);


            yield return null;
        }
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

    public void LocationSelect(int locationIndex)
    {
        battleLocation = locationIndex;
    }

    public void OpenTitleMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(titleOpenButton);
    }

    public void OpenShadeSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectOpenButton);
    }

    public void CloseShadeSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectCloseButton);
    }

    public void OpenOptionsMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionsOpenButton);
    }

    public void CloseOptionsMenu()
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

    public void SelectShade(int shadeIndex)
    {
        PlayerPrefs.SetInt("playerShadeIndex", shadeIndex);
        Debug.Log("shadeIndex: " + shadeIndex.ToString());
    }
}
