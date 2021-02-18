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
    
    //CONSTRUCTOR
    public StatModifier(float value, StatModType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, null) { }

}
