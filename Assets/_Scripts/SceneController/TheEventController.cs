using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEventController : MonoBehaviour
{

    [SerializeField] private GameObject _lookingGuard1;
    [SerializeField] private GameObject _lookingGuard2;


    private void Start()
    {
        ActivateSoldiersWithDelay();
    }

    private void ActivateSoldiersWithDelay()
    {
        Invoke(nameof(ActivateSolider1), 0.5f);
        Invoke(nameof(ActivateSolider2), 1f);
    }

    private void ActivateSolider1()
    {
        _lookingGuard1.SetActive(true);
    }
    
    private void ActivateSolider2()
    {
        _lookingGuard2.SetActive(true);
    }

    public void LoadLeadUpScene()
    {
        SceneManager.LoadScene("LeadUp");
    }
    
    private void ClimbAnimation()
    {
        
    }
    
}
