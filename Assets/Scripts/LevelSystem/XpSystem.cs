using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpSystem 
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    private int toNextLevelXP;
    private int curLevel;
    private int curXP;

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
            curXP -= toNextLevelXP;
            if (OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        }
        if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }

    public int GetXPForLevel(int level)
    {
        int xpRequired = (int)(4 * Math.Pow((double)level, 3)) / 5;

        return xpRequired;
    }

    public int GetLevelNumber()
    {
        return curLevel;
    }

    public int GetXPAmount()
    {
        return curXP;
    }

    public float GetExperienceNormalized()
    {
        return (float)curXP / toNextLevelXP;
    }
}
