using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Quests
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject tooltip;

        private GameObject _instanceTooltip;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_instanceTooltip != null)
            {
                Destroy(_instanceTooltip.gameObject);
            }
            
            _instanceTooltip = Instantiate(tooltip, GetComponentInParent<Canvas>().transform);
            
            InitTooltipContent();
            UpdateRectPosition();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(_instanceTooltip.gameObject);
        }

        private void InitTooltipContent()
        {
            var questItem = GetComponent<QuestItem>();
            var tooltipUI = _instanceTooltip.GetComponent<Toolltip>();

            if (questItem == null || questItem.Quest == null || tooltipUI == null) return;
            
            tooltipUI.SetTooltipUI(questItem.Quest);
        }

        private void UpdateRectPosition()
        {
            if (_instanceTooltip == null) return;
            
            Canvas.ForceUpdateCanvases();

            var tooltipCorners = new Vector3[4];
            var slotCorners = new Vector3[4];
            
            _instanceTooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
            GetComponent<RectTransform>().GetWorldCorners(slotCorners);

            var position = transform.position;
            var below = position.y > Screen.height / 2f;
            var right = position.x < Screen.width / 2f;

            var slotCorner = GetCornerIndex(below, right);
            var tooltipCorner = GetCornerIndex(!below, !right);
            
            _instanceTooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _instanceTooltip.transform.position;
        }
        
        private static int GetCornerIndex(bool below, bool right)
        {
            return below && !right ? 0 : !below && !right ? 1 : !below ? 2 : 3;
        }
    }
}
