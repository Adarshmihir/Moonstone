using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class Stats : MonoBehaviour
{
    public CharacterStat Strength;
    public CharacterStat Stamina;
    public CharacterStat Perception;
    public CharacterStat Intelligence;
    public CharacterStat Agility;


    private void Start()
    {
        Strength.StatTextUpdate();
        Stamina.StatTextUpdate();
        Perception.StatTextUpdate();
        Intelligence.StatTextUpdate();
        Agility.StatTextUpdate();
    }
}


