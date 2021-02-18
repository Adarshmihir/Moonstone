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

        public Stat(CharacterStat stat, GameObject GO)
        {
            charStat = stat;
            statGameObject = GO;
        }
        
    }
    
}
