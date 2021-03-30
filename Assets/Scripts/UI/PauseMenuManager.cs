using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PauseMenuManager : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject ParamsMenuUI;

    public GameObject BindingsMenuUI;

    private bool menuActive;
    // Update is called once per frame

    private void Start()
    {
        menuActive = false;
    }

    void Update()
    {
        if (!Input.GetButtonDown("MainMenu")) return;
        if (!pauseMenuUI.activeSelf)
        {
            if (BindingsMenuUI.activeSelf || ParamsMenuUI.activeSelf)
            {
                menuActive = true;
            }
            else
            {
                menuActive = false;
            }
        }
        else
        {
            menuActive = false;
        }

        if (menuActive) return;
        OpenCloseMainMenu();
        Debug.Log("Echap pressed");
    }

    public void OpenCloseMainMenu()
    {
        if(gameIsPaused)
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
        else
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }
}
