using System.Collections.Generic;
using Combat;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public float gold = 300f;

    public int copper = 10;
    public int iron = 10;
    public int silver = 10;

    public List<Weapon> weapons = new List<Weapon>();
    public List<Equipment> equipments = new List<Equipment>();
    public List<Item> items = new List<Item>();

    public void InitSingleton()
    {
        if (instance != null) return;
        instance = this;
    }

    public List<Item> GetList() {
        return items;
    }

    public List<Weapon> GetWeapons() {
        return weapons;
    }

    public List<Equipment> GetEquipments() {
        return equipments;
    }

    public List<Item> GetItems() {
        return items;
    }

    public bool Add(Item item) {
        if (items.Count >= space) {
            Debug.Log("Not enough room to store " + item.name);
            return false;
        }

        items.Add(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    public void Remove(Item item) {
        items.Remove(item);

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public static Inventory instance;

    private void Awake() {
        InitSingleton();
    }
}
