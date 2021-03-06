﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour
{
    private Text levelText;
    private Image xpBarImage;
    private XpSystem levelSystem;
    
    private void SetVariable()
    {
        levelText = transform.Find("levelText").GetComponent<Text>();
        xpBarImage = transform.Find("xpBar").Find("bar").GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("XP = " + levelSystem.GetXPAmount());
            levelSystem.AddExperience(10);
            Debug.Log("XP + 10 ");
            Debug.Log("XP = " + levelSystem.GetXPAmount());
            Debug.Log("XP next level = " + levelSystem.GetXPForLevel(levelSystem.GetLevelNumber()+1));
        }
    }
    private void SetExperienceBarSize(float xpNormalized)
    {
        xpBarImage.fillAmount = xpNormalized;
    }

    private void SetLevelNumber(int levelNumber)
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

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        SetExperienceBarSize(levelSystem.GetExperienceNormalized());
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        SetLevelNumber(levelSystem.GetLevelNumber());
    }
}
