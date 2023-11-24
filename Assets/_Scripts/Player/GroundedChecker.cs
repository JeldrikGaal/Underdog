using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedChecker : MonoBehaviour
{
    public bool Grounded { private set; get; }
    
    public static event Action OnLandedOnGround;
    public static event Action OnLeftGround;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            EnterGround();
        }
    }

    private void EnterGround()
    {
        if (!Grounded)
        {
            Grounded = true;    
            OnLandedOnGround?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            ExitGround();
        }
    }

    private void ExitGround()
    {
        Grounded = false;
        OnLeftGround?.Invoke();
    }
}
