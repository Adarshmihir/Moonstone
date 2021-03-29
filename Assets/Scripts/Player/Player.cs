using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Control;
using Resources;
using Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float life;
    public float mana;
    public List<Stat> stats;
    public int level;


    public bool isInDungeon = false;
    public bool hasKilledABoss = false;
    private static float BONUS_HEATH_PER_POINT = 5f;
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
        level = 0;
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
            if (stat.statGameObject)
            {
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

    public void AddPointToStat()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        name = name.Replace("_Button","");
        foreach (var stat in stats)
        {
            if (stat.StatName.ToString() == name)
            {
                StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
                stat.charStat.IncrementBaseValue(2);

                if (stat.StatName == StatTypes.Stamina)
                {
                    this.GetComponent<Health>().addHealthPlayer(BONUS_HEATH_PER_POINT);
                }

                statList.lvlup_Points -= 1;
                statList.PointsToSpendTextUpdate(statList.lvlup_Points);
                if (statList.lvlup_Points == 0)
                {
                    statList.ToggleLevelUp(false);
                }
                StatTextUpdate();
            }
        }

    }

    public void ResetStat()
    {
        foreach (Stat stat in stats)
        {
            if (stat.StatName == StatTypes.Stamina)
            {
                this.GetComponent<Health>().addHealthPlayer(-(stat.charStat.BaseValue - 5f)/2 * BONUS_HEATH_PER_POINT);
            }
            stat.charStat.ResetBaseValue(5);
        }
        GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>().ToggleReset(level);
        this.StatTextUpdate();
    }
}
