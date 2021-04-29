using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    private Text uiText;
    private string message;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;
    private bool invisibleChars;
    public void AddWriter(Text uiText, string message, float timePerCharacter, bool invisibleChars)
    {
        this.uiText = uiText;
        this.message = message;
        this.timePerCharacter = timePerCharacter;
        characterIndex = 0;
        this.invisibleChars = invisibleChars;
    }

    private void Update()
    {
        if(uiText != null)
        {
            timer -= Time.deltaTime;
            while(timer <= 0f)
            {
                //Display next character
                timer += timePerCharacter;
                characterIndex++;
                string text = message.Substring(0, characterIndex);
                if(invisibleChars)
                {
                    text += "<color=#00000000>" + message.Substring(characterIndex) + "</color>";
                }
                uiText.text = text;
                if(characterIndex >= message.Length)
                {
                    uiText = null;
                    return;
                }
            }
        }
    }
}
