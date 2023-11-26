using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTrap : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bricks;
    private List<Rigidbody> _rigidbodies;
    
    private void Awake()
    {
        GetRigidbodies();
    }

    private void GetRigidbodies()
    {
        _rigidbodies = new List<Rigidbody>();
        foreach (var brick in _bricks)
        {
            _rigidbodies.Add(brick.GetComponent<Rigidbody>());
        }
    }

    private void TriggerTrap()
    {
        foreach (var brick in _rigidbodies)
        {
            brick.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (UTIL.IsColliderPlayer(other))
        {
            TriggerTrap();
        }
    }
}
