using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator sceneAnimator;

        [SerializeField] private float playerToFadeTimer = 1f;
        [SerializeField] private float fadeToSkipTimer = 1f;

        [SerializeField] private PauseMenuManager settingsMenu;
        [SerializeField] private GameObject confirmMenu;
    
        // Start is called before the first frame update
        private void Start()
        {
            newGameButton.onClick.AddListener(PlayerStandUp);
            settingsButton.onClick.AddListener(SettingsPressed);
            quitButton.onClick.AddListener(QuitPressed);
        }

        private void PlayerStandUp()
        {
            if (confirmMenu.activeSelf || settingsMenu.IsMenuActive) return;
            
            StartCoroutine(StandUp());
        }

        private void QuitPressed()
        {
            if (confirmMenu.activeSelf || settingsMenu.IsMenuActive) return;

            confirmMenu.SetActive(true);
        }

        private void SettingsPressed()
        {
            if (confirmMenu.activeSelf || settingsMenu.IsMenuActive) return;
            
            settingsMenu.OpenCloseMainMenu();
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private IEnumerator StandUp()
        {
            playerAnimator.SetTrigger("standUp");
            
            yield return new WaitForSeconds(playerToFadeTimer);

            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            sceneAnimator.SetTrigger("startFade");
            
            yield return new WaitForSeconds(fadeToSkipTimer);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
