using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public GameObject SettingCanvas;

    private bool setting = false;
    private void Update()
    {
        if(setting && Input.GetButtonDown("Exit"))
        {
            setting = false;
            SettingCanvas.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SettingGame()
    {
        setting = true;
        SettingCanvas.SetActive(true);
    }
}
