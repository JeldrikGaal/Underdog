using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactText;

    private const string InteractTextContent = "Press E to interact";
    
    private void OnEnable()
    {
        BaseInteractable.PlayerEnteredRangeToInteract += ShowInteractReady;
        BaseInteractable.PlayerLeftRangeToInteract    += ClearInteractText;
    }

    private void OnDisable()
    {
        BaseInteractable.PlayerEnteredRangeToInteract -= ShowInteractReady;
        BaseInteractable.PlayerLeftRangeToInteract    -= ClearInteractText;
    }

    private void ShowInteractReady(BaseInteractable obj = null)
    {
        SetInteractText(InteractTextContent);
    }
    
    public void ClearInteractText(BaseInteractable obj = null)
    {
        _interactText.text = "";
    }
    
    public void SetInteractText(string text)
    {
        _interactText.text = text;
    }
    
    
}
