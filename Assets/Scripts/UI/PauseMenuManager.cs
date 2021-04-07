using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private bool mainMenu;
    
    public static bool gameIsPaused;

    public GameObject pauseMenuUI;

    public GameObject ParamsMenuUI;

    public GameObject BindingsMenuUI;

    public bool IsMenuActive { get; private set; }

    // Update is called once per frame
    private void Start()
    {
        IsMenuActive = false;
    }

    private void Update()
    {
        if (!Input.GetButtonDown("MainMenu") || !mainMenu) return;
        
        if (!pauseMenuUI.activeSelf)
        {
            if (BindingsMenuUI.activeSelf || ParamsMenuUI.activeSelf)
            {
                IsMenuActive = true;
            }
            else
            {
                IsMenuActive = false;
            }
        }
        else
        {
            IsMenuActive = false;
        }

        if (IsMenuActive) return;
        OpenCloseMainMenu();
    }

    public void OpenCloseMainMenu()
    {
        if (gameIsPaused)
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
