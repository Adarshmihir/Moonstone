using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chooseEquipSlot : MonoBehaviour
{
    public GameObject HelmetSlot;
    public GameObject BodySlot;
    public GameObject FootSlot;
    public GameObject WeaponSlot;

  public void addEquipment(Item equipment)
    {
        switch (equipment.equipSlot)
        {
            case EquipmentSlot.Head:
                HelmetSlot.GetComponent<EquipSlot>().AddItem(equipment);
                break;
            case EquipmentSlot.Body:
                BodySlot.GetComponent<EquipSlot>().AddItem(equipment);
                break;
            case EquipmentSlot.Legs:
                break;
            case EquipmentSlot.Foot:
                FootSlot.GetComponent<EquipSlot>().AddItem(equipment);
                break;
            case EquipmentSlot.Weapon:
                WeaponSlot.GetComponent<EquipSlot>().AddItem(equipment);
                break;
        }
    }

}
