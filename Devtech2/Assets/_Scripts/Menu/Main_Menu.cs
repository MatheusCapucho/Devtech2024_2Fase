using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private GameObject creditos;
    [SerializeField] private GameObject config;

    public void Play()
    {
        if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCount)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        else
            GoMainMenu();


    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Sair()
    {
        Application.Quit();
    }


    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

    #region activation
    //parte de ativação ode menus
    public void ActivateCredits()
    {
        if (creditos.activeSelf == true)
        {
            verifyActive();
        }
        else
        {
            verifyActive();
            creditos.SetActive(true);
        }

    }

    public void ActivateConfig()
    {
        if (config.activeSelf == true)
        {
            verifyActive();
        }
        else
        {
            verifyActive();
            config.SetActive(true);
        }


    }

    private void verifyActive()
    {
        if (creditos.activeSelf == true)
        {
            creditos.SetActive(false);

        }
        if (config.activeSelf == true)
        {
            config.SetActive(false);

        }
    }
    #endregion




}





