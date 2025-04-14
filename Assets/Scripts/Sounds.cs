using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public bool start;
    private int rand;
    public float volume;
    private float volumeSet;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        volumeSet = volume / 150;
        volume = volumeSet * GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().volumeSetting;
        audioSource.volume = volume / 10;

        if (start == true)
        {
            rand = Random.Range(0, sounds.Length);
            audioSource.PlayOneShot(sounds[rand], 1);
        }
    }

    public void Play()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        volumeSet = volume / 150;
        volume = volumeSet * GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Settings>().volumeSetting;
        audioSource.volume = volume / 10;

        rand = Random.Range(0, sounds.Length);
        audioSource.PlayOneShot(sounds[rand], 1);
    }
}
