using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType
{
    Flat = 100,
    Percent = 200,
}

public enum StatModSources
{
    LevelUp = 100,
    Item = 200,
}

public enum StatTypes
{
    Strength,
    Stamina,
    Intelligence,
    Perception,
    Agility
}

[CreateAssetMenu(fileName = "New Modifier", menuName = "Stat/StatModifier")]
public class StatModifier : ScriptableObject
{
    //PUBLIC VARIABLES
    public float Value;
    public StatModType Type;
    public object Source;
    public StatTypes statType;
    
    public void Init(float value, StatModType type, object source, StatTypes statType)
    {
        Value = value;
        Type = type;
        Source = source;
        this.statType = statType;
    }
    public static StatModifier CreateInstance(float value, StatModType type, object source, StatTypes statType)
    {
        var data = ScriptableObject.CreateInstance<StatModifier>();
        data.Init(value, type, source, statType);
        return data;
    }

    public static StatModifier CreateInstance(float value, StatModType type, StatTypes statType)
    {
        var data = ScriptableObject.CreateInstance<StatModifier>();
        data.Init(value, type, null, statType);
        return data;
    }

}
