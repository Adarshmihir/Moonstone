using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Object", menuName = "Inventory System/Items/Resource")]
public class ResourceObject : ItemObject {

    public int restoreHealthValue;
    public void Awake() {
        //type = ItemType.Resource;
    }
}
