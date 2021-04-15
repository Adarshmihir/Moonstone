using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Control;
using ResourcesHealth;
using Stats;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    GameObject UILife;
    public float life;
    public float mana;
    GameObject UIMana;
    public List<Stat> stats;
    public int level;
    public float BONUS_HEATH_PER_POINT = 5f;

    public static float bonushealth = 5f;
    public bool isInDungeon = false;
    public bool hasKilledABoss = false;

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeStats()
    {
        stats = new List<Stat>();
        StatList statList = GameManager.Instance.uiManager.StatsCanvasGO.GetComponent<StatList>();
        foreach (StatTypes stat in Enum.GetValues(typeof(StatTypes)))
        {
            Stat statToAdd = new Stat(new CharacterStat(5),
                statList.getNumberGameObject(stat), stat);
            stats.Add(statToAdd);
        }
        UILife = statList.getNumberGameObject("Health");
        UIMana = statList.getNumberGameObject("Ressources");

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
                if (stat.StatName == StatTypes.Stamina)
                {
                    this.GetComponent<Health>().addHealthPlayer(-(stat.charStat.Value - stat.charStat.BaseValue) * BONUS_HEATH_PER_POINT);
                }
                stat.charStat.AddModifier(statMod);

                if (stat.StatName == StatTypes.Stamina)
                {
                    this.GetComponent<Health>().addHealthPlayer((stat.charStat.Value - stat.charStat.BaseValue) * BONUS_HEATH_PER_POINT);
                }
            }
        }
        StatTextUpdate();
    }

    public void RemoveModifier(StatModifier statMod)
    {
        foreach (var stat in stats)
        {
            if (stat.StatName == statMod.statType)
            {
                if (stat.StatName == StatTypes.Stamina)
                {
                    this.GetComponent<Health>().addHealthPlayer(-(stat.charStat.Value - stat.charStat.BaseValue) * BONUS_HEATH_PER_POINT);
                }
                stat.charStat.RemoveModifier(statMod);

                if (stat.StatName == StatTypes.Stamina)
                {
                    this.GetComponent<Health>().addHealthPlayer((stat.charStat.Value - stat.charStat.BaseValue) * BONUS_HEATH_PER_POINT);
                }
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
        UILife.GetComponent<Text>().text = this.GetComponent<Health>().MaxHealthPoints.ToString();
        UIMana.GetComponent<Text>().text = GameObject.Find("EnergyGlobe").GetComponentInChildren<EnergyGlobeControl>().maxEnergy.ToString();
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
