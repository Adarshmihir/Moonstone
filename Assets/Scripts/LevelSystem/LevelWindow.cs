using System.Collections;
using System.Collections.Generic;
using ResourcesHealth;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Image xpBarImage;
    public XpSystem levelSystem;
    
    public void SetVariable()
    {
        levelText = transform.Find("levelText").GetComponent<Text>();
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
        xpBarImage.fillAmount = xpNormalized;
    }

    public void SetLevelNumber(int levelNumber)
    {
        levelText.text = "LEVEL" + (levelNumber + 1);
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
