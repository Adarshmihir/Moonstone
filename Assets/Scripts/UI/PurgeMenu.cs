using System;
using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;
public class PurgeMenu : MonoBehaviour
{
    public GameObject PurgeOpening;
    public GameObject PurgeEnding;
    public CanvasGroup Canvas;
    public Text counterText;
    public Timer timer;
    public UIProgressBar fill;
    public Text timerText;
    public Text timerLabel;
    public Text remainLabel;
    public Text remainText;
    public float Duration = 0.4f;
    private bool mFaded;
    public bool m_AnimateOpening;
    public bool m_AnimateEnding;
    
    
    // Start is called before the first frame update
    void Start() {
        counterText.text += fill.fillAmount + "%";
        timer = GetComponentInChildren<Timer>();
    }

    public void PlayAndFadeOpening()
    {
        if (m_AnimateOpening)
        {
            PurgeOpening.GetComponent<Animator>().Play("OpeningUnfade");
        }
        m_AnimateOpening = false;
    }
    
    public void PlayAndFadeEnding()
    {
        if (m_AnimateEnding)
        {
            PurgeEnding.GetComponent<Animator>().Play("EndingUnfade");
        }
        m_AnimateEnding = false;
    }
    
    public void Fade()
    {
        mFaded = !mFaded;
        StartCoroutine((DoFade(Canvas, Canvas.alpha, mFaded ? 1 : 0)));
    }
    
    public IEnumerator DoFade(CanvasGroup canvasgroup, float start, float end)
    {
        float counter = 0f;
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvasgroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
        }
    }

    
    //Increment fill
    public void IncrementFill(float value)
    {
        fill.fillAmount += value;
    }
    
// Update is called once per frame
    void Update() {
        counterText.text = PurgeManager.Instance.purgiumAmount + "%";

        if (!GameManager.Instance.isPurgeActive) {
            timerText.enabled = false;
            timerLabel.text = "Purge non active";

            remainLabel.enabled = false;
            remainText.enabled = false;
        }
        else {
            timerLabel.text = "Temps restant : ";
                        
            timerText.enabled = true;
            timerText.color = Color.red;

            remainLabel.enabled = true;
            remainText.enabled = true;
            remainText.text = PurgeManager.Instance.killedCount + " / " + PurgeManager.Instance.numberToKill;
        }
    }
}
