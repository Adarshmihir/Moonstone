using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Forgeron : MonoBehaviour
{
    [SerializeField] private TextWriter textWriter;
    private Text messageText;
    private Button nextButton;
    private Button repairButton;
    private Button forgeButton;
    private Button buyButton;
    private int numText = 1;

    private string text1 = "Salut à toi jeune Elu ! Bienvenue dans mon humble echoppe, je ferais tout pour t'aider à retrouver les Moonstones !";
    private string text2 = "Que puis-je faire pour toi ?";
    private string textRepair = "Reparons donc tes babioles !";
    private string textForge = "Quoi qu'tu veux forger mon p'tit ?";
    private string textBuy = "Ahahah ! Tu veux voir mon stock !?";

    public GameObject shopUI;
    public GameObject repairUI;
    public GameObject forgeUI;

    private void Awake()
    {
        messageText = transform.Find("Dialogue").Find("DialogueText").GetComponent<Text>();

        nextButton = transform.Find("Dialogue").Find("NextBtn").GetComponent<Button>();
        repairButton = transform.Find("Dialogue").Find("ReparerBtn").GetComponent<Button>();
        forgeButton = transform.Find("Dialogue").Find("ForgerBtn").GetComponent<Button>();
        buyButton = transform.Find("Dialogue").Find("AcheterBtn").GetComponent<Button>();

        repairButton.gameObject.SetActive(false);
        forgeButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);

        shopUI.SetActive(false);
        repairUI.SetActive(false);
        forgeUI.SetActive(false);
    }

    public void WriteText(int numText)
    {
        switch (numText)
        {
            case 1:
                textWriter.AddWriter(messageText, text1, 0.05f, true);
                break;

            case 2:
                textWriter.AddWriter(messageText, text2, 0.05f, true);
                break;

            case 4:
                textWriter.AddWriter(messageText, textRepair, 0.05f, true);
                break;

            case 5:
                textWriter.AddWriter(messageText, textForge, 0.05f, true);
                break;

            case 6:
                textWriter.AddWriter(messageText, textBuy, 0.05f, true);
                break;

            default:
                break;
        }
    }
    public void onNext()
    {
        numText++;
        WriteText(numText);
        if(numText == 2)
        {
            nextButton.gameObject.SetActive(false);
            repairButton.gameObject.SetActive(true);
            forgeButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(true);
        }
    }

    public void onBuy()
    {
        if(shopUI.activeSelf == false)
        {
            shopUI.SetActive(true);
            WriteText(6);
            //Debug.Log("Acheter");
        }
        else
        {
            shopUI.SetActive(false);
            WriteText(2);
        }
    }

    public void onRepair()
    {
        if (repairUI.activeSelf == false)
        {
            repairUI.SetActive(true);
            WriteText(4);
            //Debug.Log("Reparer");
        }
        else
        {
            repairUI.SetActive(false);
            WriteText(2);
        } 
    }

    public void onForge()
    {
        if (forgeUI.activeSelf == false)
        {
            forgeUI.SetActive(true);
            WriteText(5);
            //Debug.Log("Forger");
        }
        else
        {
            forgeUI.SetActive(false);
            WriteText(2);
        }
    }
}
