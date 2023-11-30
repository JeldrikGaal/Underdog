using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewsAnchorScript : MonoBehaviour
{
    [SerializeField] private List<TMP_Text> _texts;
    private int _currentText;
    [SerializeField] private float _waitTime;

    private Coroutine _currentlyRunningRoutine;

    private void Start()
    {
        StartCoroutine(CycleText());
    }

    private IEnumerator CycleText()
    {
        yield return new WaitForSeconds(_waitTime);
        MoveToNextText();
        _currentlyRunningRoutine = StartCoroutine(CycleText());
    }

    public void StartFromBeginning()
    {
        StopCoroutine(_currentlyRunningRoutine);
        _texts[_currentText].gameObject.SetActive(false);
        _currentText = _texts.Count - 1;
        _texts[_currentText].gameObject.SetActive(true);
        StartCoroutine(CycleText());
    }
    
    private void MoveToNextText()
    {
        _texts[_currentText].gameObject.SetActive(false);
        _currentText++;
        if (_currentText == _texts.Count)
        {
            _currentText = 0;
        }
        _texts[_currentText].gameObject.SetActive(true);
    }

}
