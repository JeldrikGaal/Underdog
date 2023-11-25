using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    private CheckpointSystem _checkpointSystem;

    private void Start()
    {
        _checkpointSystem = GameObject.FindWithTag("LevelManager").GetComponent<CheckpointSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _checkpointSystem.SaveCheckpoint(this.gameObject);
        }   
    }
}
