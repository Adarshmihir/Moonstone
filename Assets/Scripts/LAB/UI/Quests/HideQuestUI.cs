using UnityEngine;

namespace UI.Quests
{
    public class HideQuestUI : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
