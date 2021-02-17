using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

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

            selectedDialogue = dialogue;
            Repaint();
        }

		private void OnGUI()
		{
            if (selectedDialogue == null)
			{
                EditorGUILayout.LabelField("Aucun dialogue sélectionné.");
            }
			else
			{
                EditorGUILayout.LabelField(selectedDialogue.name);
            }
		}
	}
}
