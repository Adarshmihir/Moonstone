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
        
        [SerializeField] private List<Loot> loots;

        private Mover _mover;
        private Fighter _fighter;
        
        public List<ItemObject> ListLoot = new List<ItemObject>();
        public List<Loot> Loots
        {
            get => loots;
            set => loots = value;
        }

        public LootBag LootBag { get; private set; }
        public float MINGold => minGold;
        public float MAXGold => maxGold;

        // Start is called before the first frame update
        private void Start()
        {
            InitLoot();

            LootBag = GameManager.Instance.uiManager.LootBagGO.GetComponentInChildren<LootBag>();
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
                
                LootBag.InitLootbag(this);
                LootBag.IsLooting = false;
            }
        }

        private void InitLoot()
        {
            foreach (var temploot in loots)
            {
                if (temploot.Chance >= Random.Range(0, 100))
                {
                    ListLoot.Add(temploot.LootItem);
                }
            }
        }

        
        [Serializable]
        public class Loot
        {
            [SerializeField] private ItemObject lootItem;
            [SerializeField] [Range(0, 100)] private float chance;

            public ItemObject LootItem => lootItem;

            public float Chance => chance;

            public Loot(ItemObject _itemObject, float _chance)
            {
                lootItem = _itemObject;
                chance = _chance;

            }
        }
    }
}
