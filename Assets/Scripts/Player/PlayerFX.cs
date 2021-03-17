using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private GameObject lvlUpFX;
    [SerializeField] private GameObject walkDustFX;
    [SerializeField] private GameObject questCompletionFX;
    [SerializeField] private GameObject eatPlantFX;

    private void Start()
    {
        lvlUpFX.GetComponent<ParticleSystem>().Stop();
        walkDustFX.GetComponent<ParticleSystem>().Stop();
        questCompletionFX.GetComponent<ParticleSystem>().Stop();
        eatPlantFX.GetComponent<ParticleSystem>().Stop();
    }

    public IEnumerator StopAnim(ParticleSystem effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.Stop();
    }

    public void PlayLvLUp()
    {
        var effect = lvlUpFX.GetComponent<ParticleSystem>();
        if (!effect.isPlaying)
        {
            effect.GetComponent<Transform>().position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }    

    public void PlayWalkDust()
    {
        var effect = walkDustFX.GetComponent<ParticleSystem>();
        if (!effect.isPlaying)
        {
            effect.GetComponent<Transform>().position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 0.5f));
        }
    }    

    public void PlayQuestCompletion()
    {
        var effect = questCompletionFX.GetComponent<ParticleSystem>();
        if(!effect.isPlaying)
        {
            effect.GetComponent<Transform>().position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }    

    public void PlayEatPlant()
    {
        var effect = eatPlantFX.GetComponent<ParticleSystem>();
        if (!effect.isPlaying)
        {
            effect.GetComponent<Transform>().position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }
}
