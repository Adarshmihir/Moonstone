using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PauseMenuManager : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("MainMenu"))
        {
            OpenCloseMainMenu();
            Debug.Log("Echap pressed");
        }  
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
