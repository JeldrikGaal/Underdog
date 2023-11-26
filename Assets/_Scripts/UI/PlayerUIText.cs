using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIText : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactText;

    private const string InteractTextContent = "Press E to interact";
    private const string PickupTextContent = "Press E to pick up";
    private const string WalkTutorialText = "Press A and D to walk";
    private const string ThrowTutorialText = "Press and hold leftclick to aim, release to throw";

    private bool _blockTextChanges; 
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
        if (obj == null)
        {
            SetInteractText(InteractTextContent);
        }
        else
        {
            if (obj is PickupInteractable)
            {
                SetInteractText(PickupTextContent);
            }
        }
        
    }
    
    public void ClearInteractText()
    {
        Debug.Log("susy");
        SetInteractText("");
    }
    
    public void ClearInteractText(BaseInteractable obj)
    {
        SetInteractText("");
    }
    
    public void SetInteractText(string text)
    {
        if (_blockTextChanges)
        {
            return;
        }
                
        _interactText.text = text;
    }
    
    public void ShowWalkTutorial()
    {
        SetInteractText(WalkTutorialText);
    }
    
    public void ShowThrowTutorial()
    {
        SetInteractText(ThrowTutorialText);
        BlockTextChangesForSecond(0.5f);
        
        Invoke(nameof(ClearInteractText), 5f);
    }

    private void BlockTextChangesForSecond(float time)
    {
        BlockTextChanges();
        Invoke(nameof(UnBlockTextChanges), time);
    }
    
    private void BlockTextChanges()
    {
        _blockTextChanges = true;
    }

    private void UnBlockTextChanges()
    {
        _blockTextChanges = false;
    }
    
    
}
