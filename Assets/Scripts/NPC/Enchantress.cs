﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enchantress : Interactable
{

    protected override void checkFocus()
    {
        base.checkFocus();
        if (hasInteracted)
        {
             var distance = Vector3.Distance(base.player.position, interactionTransform.position);
            if (distance > radius)
            {
                if (GameManager.Instance.uiManager.EnchantressGO.activeSelf == true)
                {
                    GameManager.Instance.uiManager.EnchantressGO.SetActive(false);
                    GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().EnchantressMainSlotButton.GetComponent<EnchantressMainSlot>().OnRemoveButton();
                }
                this.OnDefocused();
            }
        }
    }

    public override void Interact()
    {
        base.Interact();
        if (GameManager.Instance.uiManager.EnchantressGO.activeSelf == false)
        {
            GameManager.Instance.uiManager.EnchantressGO.SetActive(true);
            GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().inventoryenchantress.Initialize_InventoryEnchantressUI();
        }
    }
}
