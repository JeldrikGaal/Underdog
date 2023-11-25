using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    // How to use the checkpoints system:
    // 1. Place all interactive / physics / rigidbody objects in _objectsTransforms list.
    // 2. Place a starting checkpoint GO - to avoid player resetting in the beginning and breaking the code.
    // 3. Assign _playerTransform
    // 4. On "R" keydown, player resets.
    // 5. OnTriggerEnter of any CheckpointGO that includes Checkpoint.cs - SaveCheckpoint() is triggered.
    
    public GameObject _lastCheckpoint;
    public Transform _playerTransform;
    public List<Transform> _objectsTransforms;


    private Vector3 _playerLastPosition = new Vector3();
    private List<Vector3> _objectsPos = new List<Vector3>();
    private List<Quaternion> _objectsRot = new List<Quaternion>();
    
    // Start is called before the first frame update
    void Start()
    {
        _playerLastPosition = _playerTransform.position;
        
        foreach(Transform objectTransform in _objectsTransforms){
            _objectsPos.Add(objectTransform.position);
            _objectsRot.Add(objectTransform.rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToLastCheckpoint();
        }
    }

    public void SaveCheckpoint(GameObject newCheckpoint)
    {
        _playerLastPosition = _playerTransform.position;
        
        for (int i = 0; i < _objectsTransforms.Count; i++)
        {
            _objectsPos[i] = _objectsTransforms[i].position;
            _objectsRot[i] = _objectsTransforms[i].rotation;
        }

        if (_lastCheckpoint != null && _lastCheckpoint!=newCheckpoint)
            _lastCheckpoint.SetActive(false);
        
        _lastCheckpoint = newCheckpoint;
        Debug.Log("Checkpoint Saved");
    }

    public void ResetToLastCheckpoint()
    {
        _playerTransform.position = _playerLastPosition;
        
        for (int i = 0; i < _objectsTransforms.Count; i++)
        {
            _objectsTransforms[i].gameObject.GetComponent<Rigidbody>().isKinematic = true;
            _objectsTransforms[i].position = _objectsPos[i];
            _objectsTransforms[i].rotation = _objectsRot[i];
            _objectsTransforms[i].gameObject.GetComponent<Rigidbody>().isKinematic = false;

        }
    }
}
