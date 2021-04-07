using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Weapon Object", menuName = "Inventory System/Items/Weapon")]
public class WeaponObject : ItemObject {
    private float weaponDamagePercent;
    private StatTypes CurrentStatUsing;
    private float weaponDamageFlat;
    
    private void Awake() {
        type = ItemType.Weapon;
        
        weaponDamagePercent = Random.Range(25, 100);
        CurrentStatUsing = (StatTypes)Random.Range(0, 4);
    }
    
    // Function calculate Dmg with flat dmg of weapon  + percent of stat of player
    public float CalculateDamageWeapon() {
        float statValue = GameManager.Instance.player.stats.Find(x => x.StatName == CurrentStatUsing).charStat.BaseValue;
        return Mathf.Round(weaponDamageFlat + (statValue * weaponDamagePercent));
    }
}
