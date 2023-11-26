using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SneakArea : MonoBehaviour
{
    public static event Action PlayerEnteredSneakArea;
    public static event Action PlayerLeftSneakArea;
    
    private void OnTriggerEnter(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            PlayerEnteredSneakArea?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            PlayerLeftSneakArea?.Invoke();
        }
    }
}

