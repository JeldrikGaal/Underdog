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

    private bool _aiming;

    [SerializeField] private Transform _throwingHandTransform;
    
    public static event Action StartAiming;
    public static event Action ReleaseAiming;
    
    private void Start()
    {
        _playerdata = PlayerController.Instance.Data;
        _playerCamera = GetCamera();
    }
    
    private void Update()
    {
       AimInput();
    }

    private void AimInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsThrowAllowed())
            {
                BeginAiming();
            }
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (IsThrowAllowed())
            {
                EndAiming();
            }
        }

        if (_aiming)
        {
            Aim();
        }
    }

    private void Aim()
    {
        // TODO: wait for kurti schmurti hurti to implement rig for aiming and then put the logic here
    }
    
    private void BeginAiming()
    {
        StartAiming?.Invoke();
        _aiming = true;
    }

    private void EndAiming()
    {
        ReleaseAiming?.Invoke();
        TryThrow();
        _aiming = false;
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
        pickedUpInteractable.DisablePhysics();
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
        _currentlyHoldingObject.EnablePhysics();
        _currentlyHoldingObject.transform.parent = null;
        _currentlyHoldingObject.SetVelocity(GetThrowDirection() * _playerdata.ThrowForce);
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
