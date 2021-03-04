using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string action;
        [SerializeField] private UnityEvent unityEvent;

        public void Trigger(string actionTrigger)
        {
            if (actionTrigger != action) return;

            unityEvent.Invoke();
        }
    }
}
