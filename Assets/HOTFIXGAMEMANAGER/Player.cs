using System;
using System.Collections;
using System.Collections.Generic;
using Control;
using Stats;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float life;
    public float mana;
    public List<Stat> stats;
    
    
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeStats()
    {
        stats = new List<Stat>();
        foreach (StatTypes stat in Enum.GetValues(typeof(StatTypes)))
        {
            Stat statToAdd = new Stat(new CharacterStat(5),
                GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>().getNumberGameObject(stat), stat);
            stats.Add(statToAdd);
        }
        StatTextUpdate();
        
    }
    public void InitializePlayer()
    {
        InitializeStats();
        
    }
    
    public void AddModifier(StatModifier statMod)
    {
        foreach (var stat in stats) {
            if (stat.StatName == statMod.statType)
            {
                stat.charStat.AddModifier(statMod);
            }
        }
        StatTextUpdate();
    }

    public void StatTextUpdate()
    {
        foreach (var stat in stats)
        {
            Debug.Log("test");
            if (stat.statGameObject)
            {
                Debug.Log("basevalue" + stat.charStat.BaseValue);
                Debug.Log("value" + stat.charStat.Value);
                if (stat.charStat.BaseValue == stat.charStat.Value)
                {
                    stat.statGameObject.GetComponent<Text>().text = stat.charStat.BaseValue.ToString();
                }
                else
                {
                    stat.statGameObject.GetComponent<Text>().text = stat.charStat.BaseValue.ToString() + " (+" + (stat.charStat.Value-stat.charStat.BaseValue).ToString()+")";
                }
            }
                
        }
            
    }
}
