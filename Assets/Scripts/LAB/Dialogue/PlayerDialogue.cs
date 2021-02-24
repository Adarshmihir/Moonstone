using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    public class PlayerDialogue : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        public string GetText()
		{
            if (dialogue == null) return "";

            return "";
		}
    }
}
