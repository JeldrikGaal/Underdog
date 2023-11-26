using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetEventTriggerBox : MonoBehaviour
{
    [SerializeField] private GameObject _jet;
    [SerializeField] private List<GameObject> _explosions;
    [SerializeField] private AnimationCurve _shakeCurve;
    [SerializeField] private AudioClip _explosionSound;

    [SerializeField] private List<float> _explosionDelays;

    private CamShake _camShake;
    private BoxCollider _boxCollider;

    private void Awake()
    {
        _camShake = Camera.main.GetComponent<CamShake>();
        _boxCollider = GetComponent<BoxCollider>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            TriggerJetEvent();
        }
    }

    private void TriggerJetEvent()
    {
        _boxCollider.enabled = false;
        _jet.SetActive(true);
        for (int i = 0; i < _explosions.Count; i++)
        {
            StartCoroutine(ExplodeAfterTime(_explosions[i], _explosionDelays[i]));
        }
    }

    private IEnumerator ExplodeAfterTime(GameObject explosion, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartExplosion(explosion);
        StartCoroutine(_camShake.CustomShake(_shakeCurve));
        /*if (_explosionSound != null)
        {
            _explosionSound.Play();
        }*/
    }

    private void StartExplosion(GameObject explosion)
    {
        explosion.SetActive(true);
    }

}
