using System;
using System.Collections.Generic;
using Combat;
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
        
        public bool bLvlupactive = false;
        public int lvlup_Points;

        private void Start()
        {
           
        }
        
        //Activate or deactivate lvl up buttons
        public void ToggleLevelUp(bool lvlupactive)
        {
            if (lvlupactive)
            {
                lvlup_Points += 1;
                PointsToSpend.GetComponent<Text>().text = lvlup_Points.ToString();
                bLvlupactive = true;
            }
        }

        public void ToggleReset(int level)
        {
            lvlup_Points = level * 1;
            
            if (lvlup_Points != 0)
            {
                PointsToSpend.GetComponent<Text>().text = lvlup_Points.ToString();
                bLvlupactive = true;
                LevelUpStatButtons.SetActive(true);
                PointsAvailable.SetActive(true);
            }
        }
        public GameObject getNumberGameObject(StatTypes type)
        {
            GameObject gameObjectToReturn = GameObject.Find(type + "_Number");
            if (gameObjectToReturn)
            {
                return gameObjectToReturn;
            }

            return null;
        }

        public GameObject getNumberGameObject(String type)
        {
            GameObject gameObjectToReturn = GameObject.Find(type + "_Number");
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


        public void AddPointToStat()
        {
            
            GameManager.Instance.player.AddPointToStat(PointsToSpend);
            
        }


        public void ResetStat()
        {
            GameManager.Instance.player.ResetStat();
        }
    }
}


