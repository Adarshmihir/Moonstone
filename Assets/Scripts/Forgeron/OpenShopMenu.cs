using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenShopMenu : MonoBehaviour
{
    public void OpenMenu()
    {
        GameManager.Instance.uiManager.ForgeronGO.SetActive(!GameManager.Instance.uiManager.ForgeronGO.activeSelf);
    }
}
