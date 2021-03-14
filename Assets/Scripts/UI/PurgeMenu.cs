using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurgeMenu : MonoBehaviour
{
    public Image fill;
    public Text counterText;
    //public float purgeIncrement; // For increment differences purposes
    private string baseCounterString = "Purge level: ";
    public Timer timer;
    
    // Start is called before the first frame update
    void Start() {
        counterText.text = baseCounterString + fill.fillAmount;
    }

// Update is called once per frame
    void Update() {
        counterText.text = PurgeManager.Instance.purgiumAmount + "%";
    }
}
