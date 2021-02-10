using System;
using UnityEngine;

namespace Stats
{
    [Serializable] 
    public class Stat : MonoBehaviour
    {
        public CharacterStat stat;

        private void Start()
        {
            stat.statGameObjectField = gameObject;
            stat.StatTextUpdate();
        }
    
    }
}
