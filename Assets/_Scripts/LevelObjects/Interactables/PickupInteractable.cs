using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : BaseInteractable
{
    public static event Action<PickupInteractable> PickupCollected;

    [SerializeField] private bool _throwable;
    
    public bool IsThrowable()
    {
        return _throwable;
}
    
    public override void Interact()
    {
        PickupCollected?.Invoke(this);
        InvokePlayerLeftRange();
        if (! IsThrowable())
        {
            Destroy(gameObject);
        }
    }
}
