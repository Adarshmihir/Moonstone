using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
	    private Dialogue _selectedDialogue;
	    private GUIStyle _guiStyle;
	    private DialogueNode _dialogueNodeDragged;
	    private Vector2 _dialogueNodeDraggedPos;

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
	            foreach (var dialogueNode in _selectedDialogue.DialogueNodes)
	            {
		            DrawConnections(dialogueNode);
	            }
				foreach (var dialogueNode in _selectedDialogue.DialogueNodes)
                {
	                DrawNode(dialogueNode);
                }
            }
		}

		private void DragDialogueEvent()
		{
			if (Event.current.type == EventType.MouseDown && _dialogueNodeDragged == null)
			{
				_dialogueNodeDragged = GetDialogueAtPos(Event.current.mousePosition);
				if (_dialogueNodeDragged == null) return;
				
				_dialogueNodeDraggedPos = _dialogueNodeDragged.rect.position - Event.current.mousePosition;
			}
			else if (Event.current.type == EventType.MouseDrag && _dialogueNodeDragged != null)
			{
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Move");
				_dialogueNodeDragged.rect.position = Event.current.mousePosition + _dialogueNodeDraggedPos;
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
	            
			EditorGUILayout.LabelField("Noeud :", EditorStyles.whiteLabel);
			var newID = EditorGUILayout.TextField(dialogueNode.id);
			var newText = EditorGUILayout.TextField(dialogueNode.text);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_selectedDialogue, "Dialogue Node Update");
				dialogueNode.id = newID;
				dialogueNode.text = newText;
			}
			
			GUILayout.EndArea();
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
