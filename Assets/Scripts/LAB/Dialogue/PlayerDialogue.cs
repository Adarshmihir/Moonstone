using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue
{
    public class PlayerDialogue : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;

        private DialogueNode _node;

        public Dialogue GetDialogue => dialogue;
        public bool IsChoosing { get; private set; }

        public event Action OnUpdate;

        public void StartDialogue(Dialogue newDialogue)
		{
            dialogue = newDialogue;
            _node = dialogue.GetRootNode();
            OnUpdate();
        }

        public string GetText()
		{
            return _node == null ? "" : _node.Text;
		}

        public IEnumerable<DialogueNode> GetChoices()
		{
            return dialogue.GetSpecificChildren(_node, true);
        }

        public void SelectChoice(DialogueNode dialogueNode)
		{
            _node = dialogueNode;
            IsChoosing = false;
            OnUpdate();
        }

        public void Next()
		{
            if (dialogue.GetSpecificChildren(_node, true).Count() > 0)
            {
                IsChoosing = true;
                OnUpdate();
                return;
			}

            var children = dialogue.GetSpecificChildren(_node, false).ToArray();
            _node = children[Random.Range(0, children.Count())];
		}

		public void Quit()
		{
            dialogue = null;
            _node = null;
            IsChoosing = false;
            OnUpdate();
        }

		public bool HasNextText()
		{
            return dialogue.GetAllChildren(_node).Count() > 0;
		}
    }
}
