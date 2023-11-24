using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Custom/PlayerData")]
public class PLAYERDATA : ScriptableObject
{
    public float Acceleration;
    public float Deceleration;
    public Vector2 MaxVelocity;
    public float AirBornAcceleartionModifier;
    public float AirBornDeceleartionModifier;
    
    [Header("Jumping")]
    public float JumpForce;
    public float JumpPeakRange;
    public float MaxJumpKeyHoldTime;
    public float JumpHoldTimeFactor;
    public float JumpGravityModifier;
    public float MinJumpGravityModifier;
    public float JumpBufferTime;
    public float KoyoteTime;

    [Header("Gravity")]
    public float StandardGravity;
    public float FallingGravity;
    
    
    [Header("KeyBindings")] 
    public List<KEYBINDINGS> Keybindings;
}
