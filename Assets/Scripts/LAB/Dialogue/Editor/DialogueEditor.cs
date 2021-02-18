using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
	    private Dialogue _selectedDialogue;
	    private GUIStyle _guiStyle;

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
				foreach (var dialogueNode in _selectedDialogue.DialogueNodes)
                {
	                OnGUINode(dialogueNode);
                }
            }
		}

		private void OnGUINode(DialogueNode dialogueNode)
		{
			GUILayout.BeginArea(dialogueNode.position, _guiStyle);
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
	}
}
