using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool isPlayerTurn;
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new List<string>();
        [SerializeField] private Rect rect = new Rect(0, 0, 200, 100);

        public string Text => text;
        public List<string> Children => children;
        public Rect Rect => rect;
        public bool IsPlayerTurn => isPlayerTurn;

#if UNITY_EDITOR
        public void SetRect(Vector2 position)
        {
            Undo.RecordObject(this, "Dialogue Node Move");
            rect.position = position;
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            Undo.RecordObject(this, "Dialogue Node Update");
            text = newText;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string child)
        {
            Undo.RecordObject(this, "Dialogue Node Link");
            children.Add(child);
            EditorUtility.SetDirty(this);
        }
        
        public void RemoveChild(string child)
        {
            Undo.RecordObject(this, "Dialogue Node Unlink");
            children.Remove(child);
            EditorUtility.SetDirty(this);
        }

        public void SetIsPlayerTurn(bool value)
        {
            Undo.RecordObject(this, "Dialogue Node Style");
            isPlayerTurn = value;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
