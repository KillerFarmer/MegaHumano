using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    public AudioClip jump;
    public AudioClip damage;

    private AudioSource soundEffectAudio;

    // Start is called before the first frame update
    void Start()
    {

        if(Instance == null){
            Instance = this;

        } else if(Instance != null){
            Destroy(gameObject);
        }
        
        AudioSource source = GetComponent<AudioSource>();
        soundEffectAudio = source;

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayOneShot(AudioClip clip){
        soundEffectAudio.PlayOneShot(clip);
    }
}
