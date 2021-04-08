using System.Collections;
using TMPro;
using UnityEngine;

public class FeedbackMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackMessage;
    [SerializeField] private float timer = 5f;

    private Coroutine _coroutine;

    public void SetMessage(string text)
    {
        feedbackMessage.alpha = 1f;
        feedbackMessage.text = text;

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(MessageAlert());
    }

    private IEnumerator MessageAlert()
    {
        yield return new WaitForSeconds(timer);

        while (feedbackMessage.alpha > 0f)
        {
            feedbackMessage.alpha -= .1f;
            yield return new WaitForSeconds(.1f);
        }
        
        feedbackMessage.alpha = 0f;
        
        StopCoroutine(_coroutine);
    }
}
