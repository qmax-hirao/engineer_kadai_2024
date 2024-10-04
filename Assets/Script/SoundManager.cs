using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSE;
    [SerializeField] private AudioClip evolutionSE;
    [SerializeField] private AudioClip fallSE;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonSE()
    {
        audioSource.PlayOneShot(buttonSE);
    }

    public void PlayEvolutionSE()
    {
        audioSource.PlayOneShot(evolutionSE);
    }

    public void PlayFallSE()
    {
        audioSource.PlayOneShot(fallSE);
    }
}
