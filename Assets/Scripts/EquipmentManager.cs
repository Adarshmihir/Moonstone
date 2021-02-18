using UnityEngine;

public class EquipmentManager : MonoBehaviour {
    
    #region Singleton
        public static EquipmentManager instance;
        private void Awake() {
            if (instance != null) Debug.LogWarning("More than one instance of EquipmentManager found !");
            instance = this;
        }
    #endregion

    public Equipment[] defaultItems;
    
    public SkinnedMeshRenderer targetMesh;
    private Equipment[] currentEquipment; // items we currently have equipped
    private SkinnedMeshRenderer[] currentMeshes;

    private Inventory inventory; // Reference to the Inventory

    // Callback for when an item is equipped / unequipped
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    public void Initialize_EquipmentManager() {
        // Initialize currentEquipment based on number of equipment slots
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        
        inventory = Inventory.instance; // Get a reference to our inventory
        currentMeshes = new SkinnedMeshRenderer[numSlots];
        
        EquipDefaultItems();
    }

    // Equip a new item
    public void Equip(Equipment newItem) {
        // Find out what slot the item fits in
        int slotIndex = (int) newItem.equipSlot;
        Unequip(slotIndex);
        Equipment oldItem = null;

        // An item has been equipped so the callback is triggered
        if (onEquipmentChanged != null) {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        
        SetEquipmentBlendShapes(newItem, 100);
        
        // Insert item into the slot
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;

        currentMeshes[slotIndex] = newMesh;
    }

    // Unequip an item with a particular index
    public Equipment Unequip(int slotIndex) {
        // Only do if an item is there
        if (currentEquipment[slotIndex] != null) {
            if (currentMeshes[slotIndex] != null) {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            // Add the item to the inventory
            Equipment oldItem = currentEquipment[slotIndex];
            SetEquipmentBlendShapes(oldItem, 0);
            inventory.Add(oldItem);

            // Remove the item from the equipment array
            currentEquipment[slotIndex] = null;
            
            // Equipment has been removed so we trigger the callback
            if (onEquipmentChanged != null) {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll() {
        for(int i=0; i<currentEquipment.Length; i++)
            Unequip(i);
         EquipDefaultItems(); //TODO: fix equiping by default. Current : Add all default items to inventory when picking up an item in the world 
    }

    void SetEquipmentBlendShapes(Equipment item, int weight) {
        /*foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions) {
            targetMesh.SetBlendShapeWeight((int)blendShape, weight);
        }*/
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.U)) UnequipAll();
    }

    void EquipDefaultItems() {
        foreach (Equipment item in defaultItems) {
            Equip(item);
        }
    }
}
