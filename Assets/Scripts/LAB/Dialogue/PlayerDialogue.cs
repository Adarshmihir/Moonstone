﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue
{
    public class PlayerDialogue : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;

        private DialogueNode _node;
        private AIDialogue _aiDialogue;

        public Dialogue GetDialogue => dialogue;
        public bool IsChoosing { get; private set; }

        public event Action OnUpdate;

        public void StartDialogue(AIDialogue aiDialogue, Dialogue newDialogue)
        {
	        _aiDialogue = aiDialogue;
            dialogue = newDialogue;
            _node = dialogue.GetRootNode();
            
            StartEnterAction();
            OnUpdate?.Invoke();
		}

        public string GetText()
		{
            return _node == null ? "" : _node.Text;
		}

        public string GetName()
        {
	        return IsChoosing ? name : _aiDialogue.name;
        }

        public IEnumerable<DialogueNode> GetChoices()
		{
            return FilterByCondition(dialogue.GetSpecificChildren(_node, true));
        }

        public void SelectChoice(DialogueNode dialogueNode)
		{
			_node = dialogueNode;
            StartEnterAction();
            IsChoosing = false;
            Next();
        }

        public void Next()
		{
            if (FilterByCondition(dialogue.GetSpecificChildren(_node, true)).Any())
            {
                IsChoosing = true;
                StartExitAction();
                OnUpdate?.Invoke();
                return;
			}

            if (HasNextText())
            {
	            var children = FilterByCondition(dialogue.GetSpecificChildren(_node, false)).ToArray();
	            StartExitAction();
	            _node = children[Random.Range(0, children.Length)];
	            StartEnterAction();
	            OnUpdate?.Invoke();
            }
            else
            {
	            Quit();
            }
		}

		public void Quit()
		{
			dialogue = null;
            StartExitAction();
            _node = null;
            IsChoosing = false;
            _aiDialogue = null;
            OnUpdate?.Invoke();
        }

		public bool HasNextText()
		{
            return FilterByCondition(dialogue.GetAllChildren(_node)).Any();
		}

		private void StartEnterAction()
		{
			if (_node == null) return;
			
			TriggerAction(_node.EnterAction);
		}

		private void StartExitAction()
		{
			if (_node == null) return;
			
			TriggerAction(_node.ExitAction);
		}

		private void TriggerAction(string action)
		{
			if (action == "") return;

			foreach (var trigger in _aiDialogue.GetComponents<DialogueTrigger>())
			{
				trigger.Trigger(action);
			}
		}

		private IEnumerable<DialogueNode> FilterByCondition(IEnumerable<DialogueNode> nodes)
		{
			return nodes.Where(currentNode => currentNode.CheckCondition(GetEvaluators()));
		}

		private IEnumerable<IEvaluator> GetEvaluators()
		{
			return GetComponents<IEvaluator>();
		}
    }
}
