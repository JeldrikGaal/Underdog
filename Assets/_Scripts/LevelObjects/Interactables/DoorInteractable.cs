using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : BaseInteractable
{
    public override void Interact()
    {
        Destroy(gameObject);
        InvokePlayerLeftRange();
    }
}
