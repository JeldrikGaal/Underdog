using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThoughts : MonoBehaviour
{
    [SerializeField] private WorldText _thoughtText;
    [SerializeField] private GameObject _thoughtBubbleObject;

    [SerializeField] private List<string> _pickedUpThought;
    [SerializeField] private const string _pickedUpThoughtText = "I picked something up";
    [SerializeField] private float _thoughtLingerTime;
    
    private void OnEnable()
    {
        PickupInteractable.PickupCollected += ShowPickedUpThought;
    }

    private void OnDisable()
    {
        PickupInteractable.PickupCollected -= ShowPickedUpThought;
    }

    private void ShowPickedUpThought(PickupInteractable obj)
    {
        SetThoughtText(_pickedUpThoughtText);
    }


    public void ClearThoughtText()
    {
        _thoughtText.ClearText();
        _thoughtBubbleObject.SetActive(false);
    }
    
    public void SetThoughtText(string text)
    {
        _thoughtBubbleObject.SetActive(true);
        float writeTime = text.Length * 0.05f;
        _thoughtText.SetTextAndTime(text, writeTime);
        _thoughtText.ShowTextByLetter();
        Invoke(nameof(ClearThoughtText), _thoughtLingerTime +  writeTime ); 
    }
}
