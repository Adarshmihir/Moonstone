using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatNumberUpdate : MonoBehaviour
{
    public StatTypes type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStatefield(int BaseValue, int ModifiedValue)
    {
        GetComponent<Text>().text = BaseValue.ToString();
        if ((ModifiedValue - BaseValue)  > 0)
        {
            GetComponent<Text>().text += " (+"+ (ModifiedValue - BaseValue) +")";
        }
    }
}
