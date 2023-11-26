using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactText;

    private const string InteractTextContent = "Press E to interact";
    private const string WalkTutorialText = "Press A and D to walk";
    private const string ThrowTutorialText = "Press and hold leftclick to aim, release to throw";
    
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
    
    public void ClearInteractText()
    {
        _interactText.text = "";
    }
    
    public void ClearInteractText(BaseInteractable obj)
    {
        _interactText.text = "";
    }
    
    public void SetInteractText(string text)
    {
        _interactText.text = text;
    }
    
    public void ShowWalkTutorial()
    {
        SetInteractText(WalkTutorialText);
        Invoke(nameof(ClearInteractText), 5f);
    }
    
    public void ShowThrowTutorial()
    {
        SetInteractText(ThrowTutorialText);
        Invoke(nameof(ClearInteractText), 5f);
    }
    
    
}
