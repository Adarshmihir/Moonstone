using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu(fileName = "Dialogue", menuName="Moonstone/New Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private List<DialogueNode> dialogueNodes = new List<DialogueNode>();
        [SerializeField] private Vector2 nodeOffset = new Vector2(250, 0);

        private readonly Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        public IEnumerable<DialogueNode> DialogueNodes => dialogueNodes;

        private void OnValidate()
        {
            _nodeLookup.Clear();
            foreach (var node in dialogueNodes)
            {
                _nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
        {
            return from child in parent.Children where _nodeLookup.ContainsKey(child) select _nodeLookup[child];
        }

        public IEnumerable<DialogueNode> GetSpecificChildren(DialogueNode currNode, bool player)
        {
            return GetAllChildren(currNode).Where(node => node.IsPlayerTurn == player);
        }

        public DialogueNode GetRootNode()
		{
            return dialogueNodes[0];
		}
        
#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            var dialogue = CreateInstance<DialogueNode>();
            dialogue.name = Guid.NewGuid().ToString();

            if (parent != null)
            {
                parent.AddChild(dialogue.name);
                dialogue.SetIsPlayerTurn(!parent.IsPlayerTurn);
                dialogue.SetRect(parent.Rect.position + nodeOffset);
                Undo.RegisterCreatedObjectUndo(dialogue, "Dialogue Node Create");
                Undo.RecordObject(this, "Dialogue Node Create");
            }
            
            dialogueNodes.Add(dialogue);
            OnValidate();
        }

        public void DeleteNode(DialogueNode node)
        {
            Undo.RecordObject(this, "Dialogue Node Delete");
            dialogueNodes.Remove(node);
            OnValidate();

            foreach (var dialogueNode in dialogueNodes)
            {
                dialogueNode.RemoveChild(node.name);
            }
            Undo.DestroyObjectImmediate(node);
        }
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (dialogueNodes.Count == 0)
            {
                CreateNode(null);
            }
            
            if (AssetDatabase.GetAssetPath(this) == "") return;
            
            foreach (var dialogueNode in dialogueNodes.Where(dialogueNode => AssetDatabase.GetAssetPath(dialogueNode) == ""))
            {
                AssetDatabase.AddObjectToAsset(dialogueNode, this);
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}
