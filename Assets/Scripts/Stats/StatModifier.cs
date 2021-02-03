using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModType
{
    Flat = 100,
    Percent = 200,
}
public class StatModifier
{
    public readonly float Value;
    public readonly StatModType Type;
    public readonly object Source;
    
    public StatModifier(float value, StatModType type, object source)
    {
        Value = value;
        Type = type;
        Source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, null) { }

}
