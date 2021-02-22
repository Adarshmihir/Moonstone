using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct equipementModifier
{
    public float value;
    public StatModType modType;
    public StatTypes statType;
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {
    public override void Use() {
        base.Use();
            
        Debug.Log("j'utilise mon equipement");
    }
}


