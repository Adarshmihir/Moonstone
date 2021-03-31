using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button quitButton;

        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator sceneAnimator;

        [SerializeField] private float playerToFadeTimer = 1f;
        [SerializeField] private float fadeToSkipTimer = 1f;
    
        // Start is called before the first frame update
        private void Start()
        {
            playButton.onClick.AddListener(PlayerStandUp);
            newGameButton.onClick.AddListener(PlayerStandUp);
            settingsButton.onClick.AddListener(OnSettingsPressed);
            quitButton.onClick.AddListener(OnQuitPressed);
        }

        private void PlayerStandUp()
        {
            StartCoroutine(StandUp());
        }

        private static void OnSettingsPressed()
        {
        
        }

        private static void OnQuitPressed()
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
