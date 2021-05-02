using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private List<EventToTrigger> eventToTriggers;

        public void Trigger(string actionTrigger)
        {
            var eventIndex = ContainAction(actionTrigger);
            
            if (eventIndex == null) return;

            eventToTriggers[(int) eventIndex].UnityEvent.Invoke();
        }

        private int? ContainAction(string action)
        {
            for (var i = 0; i < eventToTriggers.Count; i++)
            {
                if (eventToTriggers[i].Action == action)
                {
                    return i;
                }
            }
            return null;
        }

        [System.Serializable]
        private class EventToTrigger
        {
            [SerializeField] private string action;
            [SerializeField] private UnityEvent unityEvent;

            public string Action => action;
            public UnityEvent UnityEvent => unityEvent;
        }
    }
}
