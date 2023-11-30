using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneEndingScreen : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private float _timeToWait;
    [SerializeField] private Image _backgroundImage;

    private void LoadScene()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    
    public void StartEndingSequence()
    {
        _backgroundImage.DOFade(1f, _timeToWait).OnComplete(LoadScene);
    }
}
