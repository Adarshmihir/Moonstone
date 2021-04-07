using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking.Match;
using UnityEngine.UIElements;
using UnityEngine.Windows.Speech;
using Image = UnityEngine.UI.Image;

public class DisplayInventory : MonoBehaviour {
    public MouseItem mouseItem = new MouseItem();

    public GameObject inventoryPrefab;
    public InventoryObject inventory;

    private Dictionary<GameObject, InventorySlot2> itemsDisplayed = new Dictionary<GameObject, InventorySlot2>();

    // Start is called before the first frame update
    void Start() {
        CreateSlots();
    }

    // Update is called once per frame
    void Update() {
        UpdateSlots();
    }

    private void CreateSlots() {
        // Make sure the dictionary is REALLY a new dictionary
        itemsDisplayed = new Dictionary<GameObject, InventorySlot2>();

        // For every "system" item, an inventorySlot with all the needed events trigger is created
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    public void UpdateSlots() {
        foreach (KeyValuePair<GameObject, InventorySlot2> _slot in itemsDisplayed) {
            // If the slot has an item in it
            if (_slot.Value.ID >= 0) {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                    inventory.database.GetItem[_slot.Value.ID].uiDisplay;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                    _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    // Add an event to a button
    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    // Mouse enter the slot
    public void OnEnter(GameObject obj) {
        mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj)) {
            mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    // Mouse exit the slot
    public void OnExit(GameObject obj) {
        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }

    // Mouse drag begin
    public void OnDragStart(GameObject obj) {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0) {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;
        }

        mouseItem.obj = mouseObject;
        mouseItem.item = itemsDisplayed[obj];
    }

    // Mouse drag end
    public void OnDragEnd(GameObject obj) {
        if (mouseItem.hoverObj) {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[mouseItem.hoverObj]);
        }
        else {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }

        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    // Mouse drag
    public void OnDrag(GameObject obj) {
        if (mouseItem.obj != null) {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}