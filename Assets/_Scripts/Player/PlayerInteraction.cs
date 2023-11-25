using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
   private BaseInteractable _currentInteractable;

   private void OnEnable()
   {
      BaseInteractable.PlayerEnteredRangeToInteract += SetCurrentInteractable;
      BaseInteractable.PlayerLeftRangeToInteract    += SetCurrentInteractable;
   }

   private void OnDisable()
   {
      BaseInteractable.PlayerEnteredRangeToInteract -= SetCurrentInteractable;
      BaseInteractable.PlayerLeftRangeToInteract    -= SetCurrentInteractable;
   }

   public void TryInteract()
   {
      if (CanInteract())
      {
         TriggerInteractionWithCurrentInteractable();
      }
   }

   private void TriggerInteractionWithCurrentInteractable()
   {
      _currentInteractable.Interact();
   }

   private bool CanInteract()
   {
      return _currentInteractable != null;
   }
   
   private bool CanSetCurrentInteractable()
   {
      return _currentInteractable == null;
   }

   private void TrySetCurrentInteractable(BaseInteractable interactable)
   {
      if (CanSetCurrentInteractable())
      {
         SetCurrentInteractable(interactable);
      }
   }

   private void SetCurrentInteractable(BaseInteractable interactable)
   {
      _currentInteractable = interactable;
   }

}

