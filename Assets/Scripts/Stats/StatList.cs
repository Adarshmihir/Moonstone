using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Stats
{
    [Serializable]
    public class StatList : MonoBehaviour
    {
        //PUBLIC VARIABLES
        const int POINT_EACH_LEVEL = 2;
        public GameObject LevelUpStatButtons;
        public GameObject PointsAvailable;
        public GameObject PointsToSpend;
        
        public bool bLvlupactive;
        public int lvlup_Points;

        private void Start()
        {
            bLvlupactive = false;
            LevelUpStatButtons.SetActive(false);
            PointsAvailable.SetActive(false);
        }
        
        //Activate or deactivate lvl up buttons
        public void ToggleLevelUp(bool lvlupactive)
        {
            if (lvlupactive)
            {
                lvlup_Points += 1;
                PointsToSpend.GetComponent<Text>().text = lvlup_Points.ToString();
            }
            LevelUpStatButtons.SetActive(lvlupactive);
            PointsAvailable.SetActive(lvlupactive);
        }
        
        
        //each click on the button increments the value
        public void OnClickIncrementBaseValue(CharacterStat characterStat)
        {
            characterStat.IncrementBaseValue(POINT_EACH_LEVEL);
            lvlup_Points -= 1;
            PointsToSpendTextUpdate(lvlup_Points);
            if (lvlup_Points == 0)
            {
                ToggleLevelUp(false);
            }
        }
        
        //DEBUG FUNCTION FOR STAT MODIFIER PERCENT TEST
        public void OnClickEquipRandomStuff()
        {
            /*foreach (var characterStat in characterStatsList)
            {
                StatModifier MODTEST = new StatModifier(10, StatModType.Percent, StatModSources.Item);
                characterStat.stat.AddModifier(MODTEST);
            }*/
        }

        public GameObject getNumberGameObject(StatTypes type)
        {
            GameObject gameObjectToReturn = GameObject.Find(""+type.ToString()+ "_Number");
            if (gameObjectToReturn)
            {
                return gameObjectToReturn;
            }
            return null;
        }
        
        //Update the points to spend number
        public void PointsToSpendTextUpdate(int value)
        {
            if (PointsToSpend)
            {
                PointsToSpend.GetComponent<Text>().text = value.ToString();
            }
        }
        
    }
}


