using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThoughts : MonoBehaviour
{
    [SerializeField] private WorldText _thoughtText;

    [SerializeField] private List<string> _pickedUpThought;
    [SerializeField] private const string _pickedUpThoughtText = "I picked something up";
    
    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
    }

    public void ClearThoughtText()
    {
        _thoughtText.SetTextAndTime("", 0f);
    }
    
    public void SetThoughtText(string text)
    {
        _thoughtText.SetTextAndTime(text, text.Length * 0.05f);
        //  Invoke();
    }
}
