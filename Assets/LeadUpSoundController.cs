using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadUpSoundController : MonoBehaviour
{

    [SerializeField] private AudioClip leadUpTrack1; 
    [SerializeField] private AudioClip leadUpTrack2;
    private bool leadUpTrack1Done = false;

    [SerializeField] private AudioClip maleMumbleSoundNeutral;
    [SerializeField] private AudioClip femaleMumbleSoundNeutral;
    [SerializeField] private AudioClip maleMumbleSoundAngry;
    [SerializeField] private AudioClip femaleMumbleSoundAngry;
    [SerializeField] private AudioClip roomTwoShots;

    private AudioSource trackAudioSource;

    [SerializeField] private AudioSource fatherAudioSource;
    [SerializeField] private AudioSource motherAudioSource;
    [SerializeField] private AudioSource soldierAudioSource;

    [SerializeField] private float secondsUntilFireStart = 5f;
    [SerializeField] private float secondsUntilDeath = 3f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        trackAudioSource = GetComponent<AudioSource>();
        PlayTrack1();
        PlayFatherNeutral();
        PlayMotherNeutral();
    }

    void PlayTrack1()
    {
        trackAudioSource.clip = leadUpTrack1;
        trackAudioSource.Play(0);
    } 
    
    void PlayTrack2()
    {
        trackAudioSource.clip = leadUpTrack2;
        trackAudioSource.loop = true;
        trackAudioSource.Play(0);
    }
    
    public void PlayFatherAngry()
    {
        fatherAudioSource.Stop();
        fatherAudioSource.clip = maleMumbleSoundAngry;
        fatherAudioSource.Play(0);
    }
    
    public void PlayFatherNeutral()
    {
        fatherAudioSource.Stop();
        fatherAudioSource.clip = maleMumbleSoundNeutral;
        fatherAudioSource.Play(0);
    }
    
    public void PlayMotherAngry()
    {
        motherAudioSource.Stop();
        motherAudioSource.clip = femaleMumbleSoundAngry;
        motherAudioSource.Play(0);
    }
    
    public void PlayMotherNeutral()
    {
        motherAudioSource.Stop();
        motherAudioSource.clip = femaleMumbleSoundNeutral;
        motherAudioSource.Play(0);
    }

    public void StopMotherSounds()
    {
        motherAudioSource.Stop();
    }
    
    public void StopFatherSounds()
    {
        fatherAudioSource.Stop();
    }

    public IEnumerator PlaySecondRoomAudio()
    {
        yield return new WaitForSeconds(secondsUntilFireStart);
        soldierAudioSource.clip = roomTwoShots;
        soldierAudioSource.Play();
        yield return new WaitForSeconds(secondsUntilDeath);
        StopMotherSounds();
        StopFatherSounds();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trackAudioSource.clip == leadUpTrack1 && !trackAudioSource.isPlaying)
        {
            PlayTrack2();
        }
    }
}
