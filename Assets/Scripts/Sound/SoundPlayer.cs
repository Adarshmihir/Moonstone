using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource source;

    public float soundTime = 4f;

    void Start()
    {
        InvokeRepeating("PlaySound", .01f, soundTime);
    }

    // Update is called once per frame
    private void PlaySound()
    {
        source.Play();
    }
}
