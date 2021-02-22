using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    
    public int space = 20;
    
    public List<Item> items = new List<Item>();

    public bool Add(Item item) {
        if (!item.isDefaultItem) {
            if (items.Count >= space) {
                Debug.Log("Not enough room to store " + item.name);
                return false;
            }

            items.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }

        return true;
    }

    public void Remove(Item item) {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    #region Singleton

    public static Inventory instance;

    private void Awake() {
        if (instance != null) Debug.LogWarning("More than one instance of Inventory found !");
        instance = this;
    }

    #endregion
}