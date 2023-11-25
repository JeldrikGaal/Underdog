using System;
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
        _audioSource = this.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerController.Moving += PlayFootStep;
        PlayerController.NotMoving += StopAudio;
    }
    
    private void OnDisable()
    {
        PlayerController.Moving -= PlayFootStep;
        PlayerController.NotMoving -= StopAudio;
    }

    void PlayFootStep()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.clip = footsteps[UnityEngine.Random.Range(0, footsteps.Count - 1)];
            _audioSource.Play(0);
        }
    }
    
    void StopAudio()
    {
        _audioSource.Stop();
    }
}
