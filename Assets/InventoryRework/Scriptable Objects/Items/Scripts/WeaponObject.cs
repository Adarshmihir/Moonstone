using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using Random = UnityEngine.Random;

public enum WeaponType
{
    Unarmed,
    OneHanded,
    TwoHanded
}

[CreateAssetMenu (fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject {
    [SerializeField] private float weaponDamagePercent;
    [SerializeField] private StatTypes currentStatUsing;
    [SerializeField] public float weaponDamageFlat;
    
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float weaponRange = 2f;
    [SerializeField] private float attackspeed = 1f;
    [SerializeField] [Range(0f, 180f)] private float weaponRadius = 45f;

    public Weapon weapon;
    
    public float WeaponRange => weaponRange;
    public float WeaponRadius => weaponRadius;
    public float AttackSpeed => attackspeed;
    public WeaponType WeaponType => weaponType;
    
    
    private void Awake() {
        //type = ItemType.Weapon;
        
        weaponDamagePercent = Random.Range(25, 100);
        currentStatUsing = this.data.buffs[0].attribute;
    }
    
    
}
