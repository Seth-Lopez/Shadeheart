using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject exitGame;

    // Start is called before the first frame update
    void Start()
    {
        exitGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            exitGame.SetActive(true);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Title");
    }

    public void CloseMenu()
    {
        exitGame.SetActive(false);
    }
}
