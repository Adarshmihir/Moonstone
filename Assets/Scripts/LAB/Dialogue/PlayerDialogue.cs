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
            
            StartEnterAction();
            OnUpdate?.Invoke();
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
            StartEnterAction();
            IsChoosing = false;
            OnUpdate?.Invoke();
        }

        public void Next()
		{
            if (dialogue.GetSpecificChildren(_node, true).Any())
            {
                IsChoosing = true;
                StartExitAction();
                OnUpdate?.Invoke();
                return;
			}

            var children = dialogue.GetSpecificChildren(_node, false).ToArray();
            StartExitAction();
            _node = children[Random.Range(0, children.Count())];
            StartEnterAction();
            OnUpdate?.Invoke();
		}

		public void Quit()
		{
            dialogue = null;
            StartExitAction();
            _node = null;
            IsChoosing = false;
            OnUpdate?.Invoke();
        }

		public bool HasNextText()
		{
            return dialogue.GetAllChildren(_node).Any();
		}

		private void StartEnterAction()
		{
			if (_node != null && _node.EnterAction != "")
			{
				print("cc enter");
			}
		}

		private void StartExitAction()
		{
			if (_node != null && _node.ExitAction != "")
			{
				print("cc enter");
			}
		}
    }
}
