using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    [Serializable]
    public class CharacterStat
    {
        //PUBLIC VARIABLES
        public GameObject statGameObjectField;
        public float BaseValue;
        public virtual float Value
        {
            get
            {
                //base value modified only if there is a modification notified or if value is different from last base value
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }
        //PRIVATE VARIABLES
        private bool isDirty = true; //used when there is a value modification
        private float _value; //store the Stat value after calculation
        private float lastBaseValue = float.MinValue;
        
        //READONLY VARIABLES - STATMODIFIERS LISTS
        private readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;
        
    
        public void StatTextUpdate()
        {
            if (statGameObjectField)
            {
                if (BaseValue == Value)
                {
                    statGameObjectField.GetComponent<Text>().text = BaseValue.ToString();
                }
                else
                {
                    statGameObjectField.GetComponent<Text>().text = BaseValue.ToString() + " (+" + (Value-BaseValue).ToString()+")";
                }
            }
        }
        
        //CONSTRUCTOR
        public CharacterStat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }
    
        public CharacterStat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }
        
        
        //Increment base value (level up)
        public virtual void IncrementBaseValue(float value)
        {
            BaseValue += value;
        }
        public void ResetBaseValue(float value)
        {
            BaseValue = value;
        }
        //Add modifier (percent or flat) to stat
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            //statModifiers.Sort();
        }
        
        //Removes all modifiers (percent and flat) from stat
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
            StatTextUpdate();
            return didRemove;
        }
    
        //Removes modifier (percent or flat) from stat
        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            StatTextUpdate();
            return false;
        }
        
        //Calculate value with all modifiers (percent and flat)
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
}
