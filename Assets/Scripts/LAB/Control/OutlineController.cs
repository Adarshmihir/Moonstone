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
        
        private Outline outline;
        private GameObject player;
    
        // Start is called before the first frame update
        private void Start()
        {
            outline = GetComponent<Outline>();
            player = GameObject.FindWithTag("Player");
        }
        
        // Update is called once per frame
        private void Update()
        {
            UpdateOutlineType();
        }

        private void UpdateOutlineType()
        {
            if (Camera.main is null) return;

            var outlineLayer = LayerMask.GetMask("Outline");
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, outlineLayer) && hit.transform == transform)
            {
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = selectedColor;
            }
            else if (GetIsTargetOfPlayer())
            {
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = selectedColor;
            }
            else
            {
                outline.OutlineMode = Outline.Mode.OutlineHidden;
                outline.OutlineColor = nonSelectedColor;
            }
        }

        private bool GetIsTargetOfPlayer()
        {
            var health = GetComponent<Health>();
            if (player == null || player.GetComponent<Fighter>().Target == null || health.IsDead) return false;

            return player.GetComponent<Fighter>().Target.GetInstanceID() == GetComponent<Health>().GetInstanceID();
        }
    }
}
