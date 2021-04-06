using UnityEngine;

public class OpenEnchantMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenMenu()
    {
        GameManager.Instance.uiManager.EnchantressGO.SetActive(true);
        GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().inventoryenchantress.Initialize_InventoryEnchantressUI();
    }
}
