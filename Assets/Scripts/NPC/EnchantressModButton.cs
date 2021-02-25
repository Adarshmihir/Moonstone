using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantressModButton : MonoBehaviour
{
    public Text TextModButton;

    public StatModifier mod;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectButton()
    {
        GameManager.Instance.uiManager.EnchantressGO.GetComponent<EnchantressUI>().SelectButton(gameObject.GetComponent<Button>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
