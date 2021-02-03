using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class CharacterStat
{
    public GameObject statGameObjectField;
    public float BaseValue;
    public virtual float Value
    {
        get
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
                StatTextUpdate();
            }
            return _value;
        }
    }
    
    public void StatTextUpdate()
    {
        if (statGameObjectField)
        {
            statGameObjectField.GetComponent<Text>().text = Value.ToString();
        }
    }
    
    private bool isDirty = true;
    private float _value;
    private float lastBaseValue = float.MinValue;
    
    private readonly List<StatModifier> statModifiers;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }
    
    public CharacterStat(float baseValue, GameObject statField) : this()
    {
        BaseValue = baseValue;
        statGameObjectField = statField;
    }

    protected void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort();
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }   
        }

        return didRemove;
    }
    
    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];
            switch (mod.Type)
            {
                case StatModType.Flat:
                    finalValue += statModifiers[i].Value;
                    break;
                case StatModType.Percent:
                    finalValue = (BaseValue * mod.Value) + finalValue;
                    break;
                default:
                    throw new NoStatModTypeException();
            }
        }

        return (float) Math.Round(finalValue, 3);
    }


}
