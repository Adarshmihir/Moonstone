using UnityEngine;
using UnityEngine.UI;

namespace Control
{
    public class LevelBadgeController : MonoBehaviour
    {
        [SerializeField] private Text levelText;
        [SerializeField] private GameObject levelUI;

        public Vector3 initialScale;

        // TODO : See stats
        private readonly float _level = 19;

        // Start is called before the first frame update
        private void Start()
        {
            // TODO : Move to another position
            levelUI.SetActive(true);
            levelText.text = _level.ToString();

            initialScale = levelUI.gameObject.transform.localScale;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Camera.main == null) return;

            var rotation = Camera.main.transform.rotation;
            levelUI.transform.LookAt(levelUI.transform.position + rotation *- Vector3.back, rotation *- Vector3.down);
        }

        private void OnMouseEnter() {
            levelUI.gameObject.transform.localScale *= 6f;
        }

        private void OnMouseExit() {
            levelUI.gameObject.transform.localScale = initialScale;
        }
    }
}
