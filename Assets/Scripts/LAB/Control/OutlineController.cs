using Combat;
using Resources;
using UnityEngine;

namespace Control
{
    [RequireComponent(typeof(Outline))]
    public class OutlineController : MonoBehaviour
    {
        [SerializeField] private Color nonSelectedColor = new Color(1f, 0.46f, 0f);
        [SerializeField] private Color selectedColor = Color.red;
        
        private Outline _outline;
        private GameObject _player;
    
        // Start is called before the first frame update
        private void Start()
        {
            _outline = GetComponent<Outline>();

            if (this.CompareTag("Player"))
                _player = GameManager.Instance.player.gameObject;
        }
        
        // Update is called once per frame
        private void Update()
        {
            if (Camera.main is null) return;

            var outlineLayer = LayerMask.GetMask("Outline");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, outlineLayer) && hit.transform == transform)
            {
                _outline.OutlineMode = Outline.Mode.OutlineAll;
                _outline.OutlineColor = selectedColor;
            }
            else if (GetIsTargetOfPlayer())
            {
                _outline.OutlineMode = Outline.Mode.OutlineAll;
                _outline.OutlineColor = selectedColor;
            }
            else
            {
                _outline.OutlineMode = Outline.Mode.OutlineHidden;
                _outline.OutlineColor = nonSelectedColor;
            }
        }

        private bool GetIsTargetOfPlayer()
        {
            var health = GetComponent<Health>();
            if (_player == null || _player.GetComponent<Fighter>().Target == null || health.IsDead) return false;

            return _player.GetComponent<Fighter>().Target.GetInstanceID() == GetComponent<Health>().GetInstanceID();
        }
    }
}
