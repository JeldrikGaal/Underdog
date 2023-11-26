using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _anyKeyText;

    private bool _allowEndingScreen;
    
    void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(AllowEndingScreen());
    }

    private IEnumerator AllowEndingScreen()
    {
        yield return new WaitForSecondsRealtime(5);
        _anyKeyText.SetActive(true);
        _allowEndingScreen = true;
    }
    
    private void Update()
    {
        if (Input.anyKeyDown && _allowEndingScreen)
        {
            Time.timeScale = 1;
            HideLoadingScreen();
        }
    }
    
    private void HideLoadingScreen()
    {
        gameObject.SetActive(false);
    }
}
