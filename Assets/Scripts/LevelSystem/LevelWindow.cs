using System;
using System.Collections;
using System.Collections.Generic;
using DuloGames.UI;
using DuloGames.UI.Tweens;
using ResourcesHealth;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Image xpBarImage;
    public XpSystem levelSystem;public enum TextVariant
    {
        Percent,
        Value,
        ValueMax
    }
		
    public UIProgressBar bar;
    public float Duration = 5f;
    public TweenEasing Easing = TweenEasing.InOutQuint;
    public Text m_Text;
    public TextVariant m_TextVariant = TextVariant.Percent;
    public int m_TextValue = 100;
    public string m_TextValueFormat = "0";

    // Tween controls
    [NonSerialized] private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
    public LevelWindow()
    {
        if (this.m_FloatTweenRunner == null)
            this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			
        this.m_FloatTweenRunner.Init(this);
    }
    
    public void SetVariable()
    {
        levelText = transform.Find("Cadre").Find("levelText").GetComponent<Text>();
        xpBarImage = transform.Find("xpBar").Find("bar").GetComponent<Image>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown("space"))
        {
            levelSystem.AddExperience(10);
        }*/
    }
    public void SetExperienceBarSize(float xpNormalized)
    {
        this.StartTween(xpNormalized, (this.bar.fillAmount * this.Duration));
        //xpBarImage.fillAmount = xpNormalized;
    }
    protected void StartTween(float targetFloat, float duration)
    {
        if (this.bar == null)
            return;
			
        var floatTween = new FloatTween { duration = duration, startFloat = this.bar.fillAmount, targetFloat = targetFloat };
        floatTween.AddOnChangedCallback(SetFillAmount);
        floatTween.ignoreTimeScale = true;
        floatTween.easing = this.Easing;
        this.m_FloatTweenRunner.StartTween(floatTween);
    }
    
    protected void SetFillAmount(float amount)
    {
        if (this.bar == null)
            return;
			
        this.bar.fillAmount = amount;
			
        if (this.m_Text != null)
        {
            if (this.m_TextVariant == TextVariant.Percent)
            {
                this.m_Text.text = Mathf.RoundToInt(amount * 100f).ToString() + "%";
            }
            else if (this.m_TextVariant == TextVariant.Value)
            {
                this.m_Text.text = ((float)this.m_TextValue * amount).ToString(this.m_TextValueFormat);
            }
            else if (this.m_TextVariant == TextVariant.ValueMax)
            {
                this.m_Text.text =  ((float)this.m_TextValue * amount).ToString(this.m_TextValueFormat) + "/" + this.m_TextValue;
            }
        }
    }
    public void SetLevelNumber(int levelNumber)
    {
        levelText.text = (levelNumber + 1).ToString();
    }

    public void SetLevelSystem(XpSystem levelSystem)
    {
        this.levelSystem = levelSystem;
        
        SetVariable();

        SetLevelNumber(levelSystem.GetLevelNumber());

        SetExperienceBarSize(levelSystem.GetExperienceNormalized());

        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    public void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    }

    public void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        SetLevelNumber(levelSystem.GetLevelNumber());
        GameManager.Instance.player.level = levelSystem.GetLevelNumber();
        StatList statlist = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
        statlist.ToggleLevelUp(true);

        GameManager.Instance.player.GetComponent<Health>().addHealthPlayer(GameManager.Instance.player.BONUS_HEATH_PER_POINT);
        GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().addEnergyPlayer((int) GameManager.Instance.player.BONUS_HEATH_PER_POINT);
        GameManager.Instance.player.StatTextUpdate();
    }
}
