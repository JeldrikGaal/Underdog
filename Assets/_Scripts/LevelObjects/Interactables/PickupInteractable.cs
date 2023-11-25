using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupInteractable : BaseInteractable
{
    public static event Action<PickupInteractable> PickupCollected;

    [SerializeField] private bool _throwable;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    
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

    public void DisablePhysics()
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false; 
    }
    
    public void EnablePhysics()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true; 
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        _rigidbody.velocity = newVelocity;
    }
}
