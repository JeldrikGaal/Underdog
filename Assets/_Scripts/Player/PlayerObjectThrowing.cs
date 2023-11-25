using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerObjectThrowing : MonoBehaviour
{
    private PLAYERDATA _playerdata;
    private Camera _playerCamera;

    private PickupInteractable _currentlyHoldingObject;

    [SerializeField] private Transform _throwingHandTransform;
    
    private void Start()
    {
        _playerdata = PlayerController.Instance.Data;
        _playerCamera = GetCamera();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryThrow();
        }
    }
    
    private Camera GetCamera()
    {
        return Camera.main;
    }

    private void OnEnable()
    {
        PickupInteractable.PickupCollected += TryPickUpObject;
    }

    private void OnDisable()
    {
        PickupInteractable.PickupCollected -= TryPickUpObject;
    }

    private void TryPickUpObject(PickupInteractable pickedUpInteractable)
    {
        if (pickedUpInteractable.IsThrowable())
        {
            PickupObject(pickedUpInteractable);
        }
    }

    private void PickupObject(PickupInteractable pickedUpInteractable)
    {
        SetCurrentlyHoldingObject(pickedUpInteractable);
        ParentObjectToHand(pickedUpInteractable.gameObject);
        DisableObjectCollider(pickedUpInteractable);
    }
    
    private void DisableObjectCollider(PickupInteractable pickedUpInteractable)
    {
        pickedUpInteractable.GetComponentInChildren<SphereCollider>().enabled = false; // TODO: not decoupled yet 
        _currentlyHoldingObject.GetComponentInChildren<Rigidbody>().isKinematic = true;
    }

    private void ParentObjectToHand(GameObject objectToParent)
    {
        objectToParent.transform.SetParent(_throwingHandTransform);
        objectToParent.transform.localPosition = Vector3.zero;
    }
    
    private void SetCurrentlyHoldingObject(PickupInteractable newPickup)
    {
        _currentlyHoldingObject = newPickup;
    }

    private bool IsThrowAllowed()
    {
        return _currentlyHoldingObject != null;
    }
    
    private void TryThrow()
    {
        if (IsThrowAllowed())
        {
            Throw();
        }
    }

    private void Throw()
    {
        Rigidbody throwingObjectRigidbody =_currentlyHoldingObject.GetComponent<Rigidbody>();
        _currentlyHoldingObject.GetComponentInChildren<Rigidbody>().isKinematic = false;
        _currentlyHoldingObject.GetComponentInChildren<SphereCollider>().enabled = true; // TODO: not decoupled yet 
        _currentlyHoldingObject.transform.parent = null;
        throwingObjectRigidbody.velocity = GetThrowDirection() * _playerdata.ThrowForce;
        _currentlyHoldingObject = null;
        
    }

    private Vector3 GetMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _playerCamera.nearClipPlane;
        return mousePos;
    }
    
    private Vector3 GetThrowDirection()
    {
        Vector3 direction = (_throwingHandTransform.position - _playerCamera.ScreenToWorldPoint(GetMousePos()))
            .normalized;
        direction = new Vector3(direction.x * -1, direction.y, 0);
        return direction;
    }
    
}
