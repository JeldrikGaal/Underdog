using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerSoundController : MonoBehaviour
{

    public List<AudioClip> footsteps;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void PlayFootStep()
    {
        _audioSource.clip = footsteps[UnityEngine.Random.Range(0, footsteps.Count - 1)];
        _audioSource.Play(0);
    }
}
