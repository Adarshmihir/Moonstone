using System;
using Combat;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {
    
    #region Singleton
        public static EquipmentManager instance;
        private void Awake() {
            if (instance != null) Debug.LogWarning("More than one instance of EquipmentManager found !");
            instance = this;
        }
    #endregion
    
    private Item[] currentStuff; // items we currently have equipped
    private Inventory inventory; // Reference to the Inventory

    // Callback for when an item is equipped / unequipped
    public delegate void OnEquipmentChanged(Item newItem, Item oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    public void Initialize_EquipmentManager() {
        // Initialize currentEquipment based on number of equipment slots
        int numSlots = System.Enum.GetNames(typeof(Item.EquipmentSlot)).Length;
        currentStuff = new Item[numSlots];

        currentStuff[currentStuff.Length - 1] = GameManager.Instance.player.GetComponent<Fighter>().weapon;

        inventory = Inventory.instance; // Get a reference to our inventory
    }

    // Equip a new item
    public Item Equip(Item newItem) {
        // Find out what slot the item fits in
        int slotIndex = (int) newItem.equipSlot;
        Item oldItem = Unequip(slotIndex);
            
        // An item has been equipped so the callback is triggered
        if (onEquipmentChanged != null) {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
            
        // Insert item into the slot
        currentStuff[slotIndex] = newItem;
        return oldItem;
    }

    // Unequip an item with a particular index
    public Item Unequip(int slotIndex) {
        // Only do if an item is there
        if (currentStuff[slotIndex] != null) {
            
            // Add the item to the inventory
            Item oldItem = currentStuff[slotIndex];
            inventory.Add(oldItem);
            
            if (oldItem.equipSlot == Item.EquipmentSlot.Head)
            {
                GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Armor);
            }
            else if (oldItem.equipSlot == Item.EquipmentSlot.Body || oldItem.equipSlot == Item.EquipmentSlot.Legs)
            {
                GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Pet);
            }
            else if (oldItem.equipSlot == Item.EquipmentSlot.Weapon)
            {
                GameManager.Instance.player.GetComponent<FighterSpell>().UpdateSpell(null, CastSource.Weapon);
            }

            // Remove the item from the equipment array
            currentStuff[slotIndex] = null;
            
            // Equipment has been removed so we trigger the callback
            if (onEquipmentChanged != null) {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll() {
        for(int i=0; i<currentStuff.Length; i++)
            Unequip(i); 
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.U)) UnequipAll();
    }
}
