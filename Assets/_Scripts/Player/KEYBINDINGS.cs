using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/KEYBINDINGS")]
public class KEYBINDINGS : ScriptableObject
{
    public KeyCode MoveKeyLeft = KeyCode.A;
    public KeyCode MoveKeyRight = KeyCode.D;
    public KeyCode MoveKeyJump = KeyCode.Space;
    public KeyCode InteractKey = KeyCode.E;
}
