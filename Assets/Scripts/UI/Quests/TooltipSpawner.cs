using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Quests
{
    public class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject tooltip;
        [SerializeField] private bool isQuest;
        [SerializeField] private bool isLootItem;

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
            HideTooltip();
        }

        public void HideTooltip()
        {
            Destroy(_instanceTooltip.gameObject);
        }

        private void InitTooltipContent()
        {
            var tooltipUI = _instanceTooltip.GetComponent<Toolltip>();
            
            if (isQuest)
            {
                var questItem = GetComponent<QuestItem>();

                if (questItem == null || questItem.QuestStatus == null || tooltipUI == null) return;
            
                tooltipUI.SetQuestTooltipUI(questItem.QuestStatus);
            }
            else if (isLootItem)
            {
                var lootItem = MouseData.slotHoveredOver;

                if (lootItem == null)
                {
                    HideTooltip();
                    return;
                }
                
                var staticInterface = GetComponentInParent<StaticInterface>();
                var dynamicInterface = GetComponentInParent<DynamicInterface>();

                var itemObject = staticInterface != null ? staticInterface.SlotsOnInterface[lootItem].ItemObject
                        : dynamicInterface.SlotsOnInterface[lootItem].ItemObject;

                var index = string.Join("", lootItem.name.Where(char.IsDigit).ToArray());
                if (index.Length == 0)
                {
                    index = "0";
                }
                var nIndex = int.Parse(index);

                if (tooltipUI == null || itemObject == null)
                {
                    HideTooltip();
                    return;
                }
            
                tooltipUI.SetItemTooltipUI(itemObject, staticInterface != null ? staticInterface.inventory : dynamicInterface.inventory, nIndex);
            }
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
