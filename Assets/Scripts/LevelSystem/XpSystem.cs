using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpSystem 
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    private float toNextLevelXP;
    private int curLevel;
    private float curXP;

    public XpSystem()
    {
        curLevel = 0;
        curXP = 0;
        toNextLevelXP = GetXPForLevel(curLevel+1);
    }
    
    public void AddExperience(int amout)
    {
        curXP += amout;
        toNextLevelXP = GetXPForLevel(curLevel + 1);
        if (curXP >= toNextLevelXP)
        {
            curLevel++;
            GameManager.Instance.player.GetComponent<PlayerFX>().PlayLvLUp();
            //Up HP + Mana
            curXP -= toNextLevelXP;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public float GetXPForLevel(int level)
    { 
        float xpRequired = (float)(4 * Math.Pow((double)level, 3)) / 5;
        return xpRequired;
    }

    public int GetLevelNumber()
    {
        return curLevel;
    }

    public float GetXPAmount()
    {
        return curXP;
    }

    public float GetExperienceNormalized()
    {
        return (float)curXP / toNextLevelXP;
    }
}
