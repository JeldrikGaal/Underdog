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

    public int MaxHealth;
    
    [Header("Jumping")]
    public float JumpForce;
    public float JumpPeakRange;
    public float MaxJumpKeyHoldTime;
    public float JumpHoldTimeFactor;
    public float JumpGravityModifier;
    public float MinJumpGravityModifier;
    public float JumpBufferTime;
    public float KoyoteTime;

    [Header("Walljumping")]
    public float HorizontalWallJumpForce;
    public float VerticalWallJumpForce;
    public float WallJumpCooldown;
    
    public float WallJumpAccelMovementModifierTime;
    public float WallJumpAccelerationModifier;
    
    public float WallJumpDecelMovementModifierTime;
    public float WallJumpDeceleartionModifier;
    
    
    [Header("Doublejumping")]
    public float DoubleJumpForce;
    public float MaxDoubleJumpVelocityPercentage;

    [Header("HangTime")] 
    public float MaxHangTime;
    public float HangTimeThreshold;

    [Header("Dash")] 
    public float DashPower;
    public float DashCooldown;
    public float DashDuration;
    public float DashMaxSpeedFactor;
    
    [Header("Gravity")]
    public float StandardGravity;
    public float FallingGravity;
    
    [Header("Gun")]
    public float weaponSwapCooldown;

    [Header("Color Flashing")] 
    public Color HealFlashColor;
    public float HealFlashDuration;
    public Color DamageFlashColor;
    public float DamageFlashDuration;
    
    
    [Header("KeyBindings")] 
    public List<KEYBINDINGS> Keybindings;
}
