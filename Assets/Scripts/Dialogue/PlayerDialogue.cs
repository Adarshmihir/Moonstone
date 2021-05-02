using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Core;
using Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue
{
    public class PlayerDialogue : MonoBehaviour, IAction
    {
        [SerializeField] private Dialogue dialogue;

        private DialogueNode _node;
        private AIDialogue _aiDialogue;
        private Fighter _fighter;
        private Mover _mover;
        private bool _isOpeningDialogue;

        public Dialogue GetDialogue => dialogue;
        public bool IsChoosing { get; private set; }

        public event Action OnUpdate;

        private void Start()
        {
	        _fighter = GetComponent<Fighter>();
	        _mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        private void Update()
        {
	        if (_aiDialogue == null) return;

	        // Check if target is not too far
	        if (!_fighter.GetIsInRange(_aiDialogue.transform.position, 2f))
	        {
		        // Move towards the target until it is close enough
		        _mover.MoveTo(_aiDialogue.transform.position);
	        }
	        else if (_isOpeningDialogue)
	        {
		        // Cancel movement action and start attack
		        _mover.Cancel();
		        _isOpeningDialogue = false;
		        
		        StartEnterAction();
		        OnUpdate?.Invoke();
	        }
        }

        public void StartDialogue(AIDialogue aiDialogue, Dialogue newDialogue)
        {
	        GetComponent<ActionScheduler>().StartAction(this);
	        
	        _aiDialogue = aiDialogue;
	        dialogue = newDialogue;
	        _node = dialogue.GetRootNode();
	        _isOpeningDialogue = true;
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

		public void Cancel()
		{
			GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().CloseMenu();
			GameManager.Instance.uiManager.ForgeronGO.SetActive(false);
			
			Quit();
		}
    }
}
