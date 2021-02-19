using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
	    private Dialogue _selectedDialogue;
	    private Vector2 _scrollPosition;
	    
	    [NonSerialized] private GUIStyle _guiStyle;
	    [NonSerialized] private DialogueNode _dialogueNodeDragged;
	    [NonSerialized] private Vector2 _dialogueNodeDraggedPos;
	    [NonSerialized] private Vector2 _canvasDraggedPos;
	    [NonSerialized] private DialogueNode _addingNode;
	    [NonSerialized] private DialogueNode _removingNode;
	    [NonSerialized] private DialogueNode _linkingNode;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditor()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenDialogue(int instanceID, int line)
        {
            var dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue == null) return false;

            ShowDialogueEditor();
            return true;
        }

		private void OnEnable()
		{
            Selection.selectionChanged += OnSelectionChanged;
            _guiStyle = new GUIStyle
            {
	            normal =
	            {
		            background = EditorGUIUtility.Load("node0") as Texture2D,
		            textColor = Color.white
	            }, 
	            padding = new RectOffset(20, 20, 20, 20),
	            border = new RectOffset(12, 12, 12, 12),
            };
		}

        private void OnSelectionChanged()
		{
            var dialogue = Selection.activeObject as Dialogue;
            if (dialogue == null) return;

            _selectedDialogue = dialogue;
            Repaint();
        }

		private void OnGUI()
		{
            if (_selectedDialogue == null)
			{
                EditorGUILayout.LabelField("Aucun dialogue sélectionné.");
            }
			else
            {
	            DragDialogueEvent();
	            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
	            GUILayoutUtility.GetRect(5000, 5000);
	            
	            foreach (var dialogueNode in _selectedDialogue.DialogueNodes)
	            {
		            DrawConnections(dialogueNode);
	            }
				foreach (var dialogueNode in _selectedDialogue.DialogueNodes)
                {
	                DrawNode(dialogueNode);
                }
				
				EditorGUILayout.EndScrollView();

				if (_addingNode != null)
				{
					Undo.RecordObject(_selectedDialogue, "Dialogue Node Create");
					_selectedDialogue.CreateNode(_addingNode);
					_addingNode = null;
				}

				if (_removingNode == null) return;
				
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Delete");
				_selectedDialogue.DeleteNode(_removingNode);
				_removingNode = null;
            }
		}

		private void DragDialogueEvent()
		{
			if (Event.current.type == EventType.MouseDown && _dialogueNodeDragged == null)
			{
				_dialogueNodeDragged = GetDialogueAtPos(Event.current.mousePosition + _scrollPosition);
				if (_dialogueNodeDragged != null)
				{
					_dialogueNodeDraggedPos = _dialogueNodeDragged.rect.position - Event.current.mousePosition;
				}
				else
				{
					_canvasDraggedPos = Event.current.mousePosition + _scrollPosition;
				}
			}
			else if (Event.current.type == EventType.MouseDrag)
			{
				if (_dialogueNodeDragged != null)
				{
					Undo.RecordObject(_selectedDialogue, "Dialogue Node Move");
					_dialogueNodeDragged.rect.position = Event.current.mousePosition + _dialogueNodeDraggedPos;
				}
				else
				{
					_scrollPosition = _canvasDraggedPos - Event.current.mousePosition;
				}
				Repaint();
			}
			else if (Event.current.type == EventType.MouseUp && _dialogueNodeDragged != null)
			{
				_dialogueNodeDragged = null;
			}
		}

		private void DrawNode(DialogueNode dialogueNode)
		{
			GUILayout.BeginArea(dialogueNode.rect, _guiStyle);
			EditorGUI.BeginChangeCheck();
			
			var newText = EditorGUILayout.TextField(dialogueNode.text);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Update");
				dialogueNode.text = newText;
			}

			DrawButtons(dialogueNode);
			
			GUILayout.EndArea();
		}

		private void DrawButtons(DialogueNode dialogueNode)
		{
			GUILayout.BeginHorizontal();
			
			if (GUILayout.Button("Ajouter"))
			{
				_addingNode = dialogueNode;
			}

			DrawLinkButtons(dialogueNode);

			if (GUILayout.Button("Supprimer"))
			{
				_removingNode = dialogueNode;
			}
			
			GUILayout.EndHorizontal();
		}
		
		private void DrawLinkButtons(DialogueNode dialogueNode)
		{
			if (_linkingNode == null)
			{
				if (!GUILayout.Button("Lier")) return;
				
				_linkingNode = dialogueNode;
			}
			else if (_linkingNode == dialogueNode)
			{
				if (!GUILayout.Button("Annuler")) return;
				
				_linkingNode = null;
			}
			else if (_linkingNode.children.Contains(dialogueNode.id))
			{
				if (!GUILayout.Button("Délier")) return;
				
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Unlink");
				_linkingNode.children.Remove(dialogueNode.id);
				_linkingNode = null;
			}
			else
			{
				if (!GUILayout.Button("Enfant")) return;
				
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Link");
				_linkingNode.children.Add(dialogueNode.id);
				_linkingNode = null;
			}
		}

		private void DrawConnections(DialogueNode dialogueNode)
		{
			var startPosition = new Vector2(dialogueNode.rect.xMax, dialogueNode.rect.center.y);
			foreach (var dialogue in _selectedDialogue.GetAllChildren(dialogueNode))
			{
				var endPosition = new Vector2(dialogue.rect.xMin, dialogue.rect.center.y);
				var offset = endPosition - startPosition;
				offset.x *= 0.8f;
				offset.y = 0;
				Handles.DrawBezier(startPosition, endPosition, startPosition + offset, endPosition - offset, Color.white, null, 4f);
			}
		}

		private DialogueNode GetDialogueAtPos(Vector2 mousePosition)
		{
			DialogueNode dialogueNode = null;
			foreach (var node in _selectedDialogue.DialogueNodes)
			{
				if (!node.rect.Contains(mousePosition)) continue;

				dialogueNode = node;
			}
			return dialogueNode;
		}
    }
}
