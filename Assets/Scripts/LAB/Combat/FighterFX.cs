using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterFX : MonoBehaviour
{
    [SerializeField] private GameObject bloodFX;


    private void Start()
    {
        bloodFX.GetComponent<ParticleSystem>().Stop();
    }

    public IEnumerator StopAnim(ParticleSystem effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.Stop();
    }

    public void PlayBleed()
    {
        var effect = bloodFX.GetComponent<ParticleSystem>();
        if (!effect.isPlaying)
        {
            effect.GetComponent<Transform>().position = this.GetComponent<Transform>().position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }
}
