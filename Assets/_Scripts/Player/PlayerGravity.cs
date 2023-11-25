using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerGravity : MonoBehaviour
{
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object
 
    private float _gravityScale = 1.0f;
 
    [SerializeField] private static readonly float _globalGravity = -9.81f;
 
    private Rigidbody _rigidbody;
    
    void OnEnable ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }
 
    void FixedUpdate ()
    {
        Vector3 gravity = _globalGravity * _gravityScale * Vector3.up;
        _rigidbody.AddForce(gravity, ForceMode.Acceleration);
    }
    
    public void SetGravityScale(float gravityScale)
    {
        _gravityScale = gravityScale;
    }
}
