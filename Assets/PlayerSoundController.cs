using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayerSoundController : MonoBehaviour
{

    public List<AudioClip> footsteps;
    public List<AudioClip> jumpClips;
    public List<AudioClip> landClips;
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
        /*PlayerController.StartedJump += JumpSound;
        PlayerController.EndedJump += LandSound;*/
    }
    
    private void OnDisable()
    {
        PlayerController.Moving -= PlayFootStep;
        PlayerController.NotMoving -= StopAudio;
        /*PlayerController.StartedJump -= JumpSound;
        PlayerController.EndedJump -= LandSound;*/
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

    public void JumpSound()
    {
        _audioSource.clip = jumpClips[UnityEngine.Random.Range(0, jumpClips.Count - 1)];
        _audioSource.Play(0);
    }
    
    public void LandSound()
    {
        if (landClips.Count == 1) _audioSource.clip = landClips[0];
        else _audioSource.clip = landClips[UnityEngine.Random.Range(0, landClips.Count - 1)];
        _audioSource.Play(0);
    }
}
