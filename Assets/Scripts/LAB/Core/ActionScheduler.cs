using UnityEngine;

namespace Core
{
    public class ActionScheduler : MonoBehaviour
    {
        public IAction CurrentAction { get; private set; }

        public void StartAction(IAction action)
        {
            if (CurrentAction == action)
            {
                return;
            }

            CurrentAction?.Cancel();
            CurrentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
