using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelWindow levelWindow;

    public void InitializeLevelManager()
    {
        XpSystem levelSystem = new XpSystem();
        levelWindow.SetLevelSystem(levelSystem);
        Debug.Log(levelWindow.levelSystem);
    }
}
