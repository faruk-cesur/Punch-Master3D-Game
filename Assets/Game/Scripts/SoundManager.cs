using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioClip collectableSound;
    [SerializeField] private AudioClip punchSound;
    [SerializeField] private AudioClip finishSound;
    [SerializeField] private AudioClip gameOverSound;

    public static SoundManager instance;
    private AudioSource audioSource;
    public bool sound;

    private void Start()
    {
        
    }

    
    private void Awake()
    {
        makeSingleton();
        audioSource = GetComponent<AudioSource>();
    }
    private void makeSingleton()
    {
        if (instance !=null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    
    private void Update()
    {
        
    }

    public void SoundOnOff()
    {
        sound = !sound;
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        if (sound)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
