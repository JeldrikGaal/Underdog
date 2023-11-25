using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BaseInteractable : MonoBehaviour
{
    public static event Action<BaseInteractable> PlayerEnteredRangeToInteract;
    public static event Action<BaseInteractable> PlayerLeftRangeToInteract;

    private void OnTriggerEnter(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            InvokePlayerEnteredRange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            InvokePlayerLeftRange();
        }
    }

    public virtual void Interact()
    {
        Debug.Log("I just got interacted with and my name is " + gameObject.name + "!");
    }

    protected void InvokePlayerEnteredRange()
    {
        PlayerEnteredRangeToInteract?.Invoke(this);
    }

    protected void InvokePlayerLeftRange()
    {
        PlayerLeftRangeToInteract?.Invoke(this);
    }
}
