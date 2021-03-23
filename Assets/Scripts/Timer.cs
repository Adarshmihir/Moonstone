using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float timeRemaining;
    public bool timerIsRunning = false;
    public Text timerText;

    public float defaultTime;

    private void Start() {
        defaultTime = 300f;
        timeRemaining = defaultTime;
    }

    void Update() {
        if (timerIsRunning) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            // Time's out
            else {
                Debug.Log("Time's out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
        else {
            timerIsRunning = false;
            timeRemaining = defaultTime;
        }
        
        if(timerText.IsActive() && GameManager.Instance.isPurgeActive)
            timerIsRunning = true;
        else
            timerIsRunning = false;
    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}