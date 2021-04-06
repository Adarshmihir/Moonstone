using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    [Serializable] 
    public class Stat
    {
        public CharacterStat charStat;
        public GameObject statGameObject;
        public StatTypes StatName;

        public Stat(CharacterStat stat, GameObject GO, StatTypes StatName)
        {
            charStat = stat;
            statGameObject = GO;
            this.StatName = StatName;
        }
        
    }
    
}
