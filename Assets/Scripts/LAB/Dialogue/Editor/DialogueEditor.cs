using UnityEditor;
using UnityEditor.Callbacks;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
	    private Dialogue _selectedDialogue;

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
	                var newText = EditorGUILayout.TextField(dialogueNode.text);
	                if (newText == dialogueNode.text) continue;

	                dialogueNode.text = newText;
	                EditorUtility.SetDirty(_selectedDialogue);
                }
            }
		}
	}
}
