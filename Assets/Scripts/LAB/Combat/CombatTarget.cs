using System;
using System.Collections.Generic;
using System.Linq;
using Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        [SerializeField] private float minGold;
        [SerializeField] private float maxGold;
        
        [SerializeField] private List<Loot> loots = new List<Loot>();
        
        private List<Item> _items = new List<Item>();

        public IEnumerable<Item> Items => _items;
        
        // Start is called before the first frame update
        private void Start()
        {
            InitLoot();
            
            if (!(minGold > maxGold)) return;
            
            var temp = minGold;
            minGold = maxGold;
            maxGold = temp;
        }

        private void InitLoot()
        {
            _items = (from loot in loots where Random.Range(0, 100) <= loot.Chance select loot.Item).ToList();
        }
        
        public void ShowLootMenu()
        {
            GameManager.Instance.uiManager.LootBag.GetComponent<LootBag>().ShowLootBag(_items, this);
        }

        public void DeleteItem(Item itemToDelete)
        {
            _items.Remove(itemToDelete);
        }
        
        [Serializable]
        public class Loot
        {
            [SerializeField] private Item item;
            [SerializeField] [Range(0, 100)] private float chance;

            public float Chance => chance;
            public Item Item => item;
        }
    }
}
