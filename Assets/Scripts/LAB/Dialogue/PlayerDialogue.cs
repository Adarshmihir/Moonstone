using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class PlayerDialogue : MonoBehaviour
    {
        [SerializeField] private Dialogue dialogue;

        private DialogueNode _node;

		private void Awake()
		{
            _node = dialogue.GetRootNode();
		}

		// Start is called before the first frame update
		private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        public string GetText()
		{
            if (_node == null) return "";

            return _node.Text;
		}

        public void Next()
		{
            var children = dialogue.GetAllChildren(_node).ToArray();
            _node = children[Random.Range(0, children.Count())];
		}

        public bool HasNextText()
		{
            return dialogue.GetAllChildren(_node).Count() > 0;
		}
    }
}
