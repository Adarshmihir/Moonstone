using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource purgeThemeSource;
    public AudioSource purgeAlert;
    private AudioSource[] sources;
    public bool alertHasBeenPlayed = false;
    
    
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()  {
        if (GameManager.Instance.isPurgeActive) {
            // Alarm played at the beginning of the Purge
            if (!purgeAlert.isPlaying && !alertHasBeenPlayed) {
                purgeAlert.Play();
                alertHasBeenPlayed = true;
            }
            else
                // Theme playing during Purge
                if(!purgeAlert.isPlaying && !purgeThemeSource.isPlaying)
                    purgeThemeSource.Play();
        }
        else {
            sources = GetComponents<AudioSource>();
            foreach (AudioSource audioSource in sources) {
                audioSource.Stop();
            }

            alertHasBeenPlayed = false;
        }
    }
}
