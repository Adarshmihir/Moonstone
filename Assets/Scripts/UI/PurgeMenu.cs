using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurgeMenu : MonoBehaviour
{
    public Image fill;
    public Text counterText;
    public Timer timer;

    public Text timerText;
    public Text timerLabel;

    public Text remainLabel;
    public Text remainText;
    
    // Start is called before the first frame update
    void Start() {
        counterText.text += fill.fillAmount;
        timer = GetComponentInChildren<Timer>();
    }

// Update is called once per frame
    void Update() {
        counterText.text = PurgeManager.Instance.purgiumAmount + "%";

        if (!GameManager.Instance.isPurgeActive) {
            timerText.enabled = false;
            timerLabel.text = "Purge not active !";

            remainLabel.enabled = false;
            remainText.enabled = false;
        }
        else {
            timerLabel.text = "Time remaining : ";
                        
            timerText.enabled = true;
            timerText.color = Color.red;

            remainLabel.enabled = true;
            remainText.enabled = true;
            remainText.text = PurgeManager.Instance.killedCount + " / " + PurgeManager.Instance.numberToKill;
        }
    }
}
