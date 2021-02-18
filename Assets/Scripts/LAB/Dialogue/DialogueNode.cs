using UnityEngine;

namespace Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string id;
        public string text;
        public string[] children;
        public Rect rect = new Rect(0, 0, 0, 0);
    }
}
