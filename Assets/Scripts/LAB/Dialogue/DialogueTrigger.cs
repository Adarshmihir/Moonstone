using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string action;
        [SerializeField] private UnityEvent unityEvent;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Trigger(string actionTrigger)
        {
            if (actionTrigger != action) return;

            unityEvent.Invoke();
        }
    }
}
