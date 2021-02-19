using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName="Moonstone/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        public IEnumerable<DialogueNode> DialogueNodes => dialogueNodes;

#if UNITY_EDITOR
        private void Awake()
        {
            if (dialogueNodes.Count != 0) return;
            
            var dialogue = new DialogueNode {id = Guid.NewGuid().ToString()};
            dialogueNodes.Add(dialogue);
        }
#endif

        private void OnValidate()
        {
            _nodeLookup.Clear();
            foreach (var node in dialogueNodes)
            {
                _nodeLookup[node.id] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            return from child in parent.children where _nodeLookup.ContainsKey(child) select _nodeLookup[child];
        }
        
        public void CreateNode(DialogueNode parent)
        {
            var dialogue = new DialogueNode {id = Guid.NewGuid().ToString()};
            parent.children.Add(dialogue.id);
			dialogueNodes.Add(dialogue);
            OnValidate();
        }

        public void DeleteNode(DialogueNode node)
        {
            dialogueNodes.Remove(node);
            OnValidate();

            foreach (var dialogueNode in dialogueNodes)
            {
                dialogueNode.children.Remove(node.id);
            }
        }
    }
}
