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

public class StatModifier
{
    //PUBLIC VARIABLES
    public readonly float Value;
    public readonly StatModType Type;
    public readonly object Source;
    
    //CONSTRUCTOR
    public StatModifier(float value, StatModType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, null) { }

}
