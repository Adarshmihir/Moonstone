using System.Collections;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem lvlUpFX;
    [SerializeField] private ParticleSystem walkDustFX;
    [SerializeField] private ParticleSystem questCompletionFX;
    [SerializeField] private ParticleSystem eatPlantFX;

    private void Start()
    {
        lvlUpFX.Stop();
        walkDustFX.Stop();
        questCompletionFX.Stop();
        eatPlantFX.Stop();
    }

    public IEnumerator StopAnim(ParticleSystem effect, float time)
    {
        yield return new WaitForSeconds(time);
        effect.Stop();
    }

    public void PlayLvLUp()
    {
        var effect = lvlUpFX;
        if (!effect.isPlaying)
        {
            effect.transform.position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }    

    public void PlayWalkDust()
    {
        var effect = walkDustFX;
        if (!effect.isPlaying)
        {
            effect.transform.position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 0.5f));
        }
    }    

    public void PlayQuestCompletion()
    {
        var effect = questCompletionFX;
        if(!effect.isPlaying)
        {
            effect.transform.position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }    

    public void PlayEatPlant()
    {
        var effect = eatPlantFX;
        if (!effect.isPlaying)
        {
            effect.transform.position = GameManager.Instance.player.transform.position;
            effect.Play();
            StartCoroutine(StopAnim(effect, 3f));
        }
    }
}
