using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [SerializeField] public AudioClip backgroundMusic;
    [SerializeField] public AudioClip death;
    [SerializeField] public AudioClip levelCompleted;
    [SerializeField] public AudioClip enemyDamage;
    [SerializeField] public AudioClip playerDamage;
    [SerializeField] public AudioClip dash;

    public bool isMusicPlaying = true;

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        isMusicPlaying = true;
    }

    public void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
        isMusicPlaying = false;
    }

    public void StartMusic()
    {
        musicSource.Play();
        isMusicPlaying = true;
    }
}
