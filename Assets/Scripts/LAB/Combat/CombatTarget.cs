using System;
using System.Collections.Generic;
using System.Linq;
using Movement;
using ResourcesHealth;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        [SerializeField] private float minGold;
        [SerializeField] private float maxGold;
        
        [SerializeField] private List<Loot> loots = new List<Loot>();

        private Mover _mover;
        private Fighter _fighter;
        private List<Item> _items = new List<Item>();

        public IEnumerable<Item> Items => _items;
        public LootBag LootBag { get; private set; }
        public float MINGold => minGold;
        public float MAXGold => maxGold;

        // Start is called before the first frame update
        private void Start()
        {
            InitLoot();

            LootBag = GameManager.Instance.uiManager.LootBagGO.GetComponent<LootBag>();
            _mover = GameManager.Instance.player.GetComponent<Mover>();
            _fighter = GameManager.Instance.player.GetComponent<Fighter>();
            
            if (!(minGold > maxGold)) return;
            
            var temp = minGold;
            minGold = maxGold;
            maxGold = temp;
        }

        private void Update()
        {
            if (!LootBag.IsLooting) return;

            if (!_fighter.GetIsInRange(transform.position, 2f))
            {
                _mover.MoveTo(transform.position);
            }
            else if (!LootBag.IsLootBagOpen())
            {
                _mover.Cancel();
                LootBag.ShowLootBag(_items, this);
                LootBag.IsLooting = false;
            }
        }

        private void InitLoot()
        {
            _items = (from loot in loots where Random.Range(0, 100) <= loot.Chance select loot.Item).ToList();
        }

        public void DeleteItem(Item itemToDelete)
        {
            _items.Remove(itemToDelete);

            if (_items.Count > 0) return;
            
            LootBag.Cancel();

            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
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
