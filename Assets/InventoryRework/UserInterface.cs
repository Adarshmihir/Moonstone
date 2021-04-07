using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public abstract class UserInterface : MonoBehaviour {
    public Player2 player;

    public InventoryObject inventory;

    protected Dictionary<GameObject, InventorySlot2> itemsDisplayed = new Dictionary<GameObject, InventorySlot2>();

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < inventory.Container.Items.Length; i++) {
            inventory.Container.Items[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // Update is called once per frame
    void Update() {
        UpdateSlots();
    }

    public abstract void CreateSlots();

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
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    // Mouse enter the slot
    public void OnEnter(GameObject obj) {
        player.mouseItem.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj)) {
            player.mouseItem.hoverItem = itemsDisplayed[obj];
        }
    }

    // Mouse exit the slot
    public void OnExit(GameObject obj) {
        player.mouseItem.hoverObj = null;
        player.mouseItem.hoverItem = null;
    }

    public void OnEnterInterface(GameObject obj) {
        player.mouseItem.ui = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj) {
        player.mouseItem.ui = null;
    }

    // Mouse drag begin
    public void OnDragStart(GameObject obj) {
        if (itemsDisplayed[obj].ID >= 0) {
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(transform.parent);

            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].ID].uiDisplay;
            img.raycastTarget = false;


            player.mouseItem.obj = mouseObject;
            player.mouseItem.item = itemsDisplayed[obj];
        }
    }

    // Mouse drag end
    public void OnDragEnd(GameObject obj) {
        var itemOnMouse = player.mouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var GetItemObject = inventory.database.GetItem;

        if (itemOnMouse.ui != null) {
            if (mouseHoverObj) {
                if (itemOnMouse.obj == null)
                    return;

                if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.item.Id <= -1 || mouseHoverItem.item.Id >= 0 && mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID])))
                    inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[mouseHoverObj]);
            }
        }
        else {
            inventory.RemoveItem(itemsDisplayed[obj].item);
        }


        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }

    // Mouse drag
    public void OnDrag(GameObject obj) {
        if (player.mouseItem.obj != null) {
            player.mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
}

// Graphical representation of the item we are moving
public class MouseItem {
    public UserInterface ui;
    public GameObject obj;
    public InventorySlot2 item;
    public InventorySlot2 hoverItem;
    public GameObject hoverObj;
}