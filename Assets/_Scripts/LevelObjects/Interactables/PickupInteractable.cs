using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : BaseInteractable
{
    
    public static event Action<PickupInteractable> PickupCollected;
    
    public override void Interact()
    {
        PickupCollected?.Invoke(this);
        InvokePlayerLeftRange();
        Destroy(gameObject, 0.25f);
    }
}
