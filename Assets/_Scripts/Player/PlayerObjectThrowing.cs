using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(PlayerController))]
public class PlayerObjectThrowing : MonoBehaviour
{
    private PLAYERDATA _playerdata;
    private Camera _playerCamera;

    private PickupInteractable _currentlyHoldingObject;

    private bool _aiming;

    [SerializeField] private Transform _throwingHandTransform;
    [SerializeField] private Transform _aimingRigTransform;
    [SerializeField] private Rig _aimingRig;
    
    public static event Action StartAiming;
    public static event Action ReleaseAiming;
    public static event Action AimingRight;
    public static event Action AimingLeft;
    
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
        _aimingRigTransform.transform.position = _throwingHandTransform.position + GetThrowDirection() * 5f;
        Debug.DrawLine(_throwingHandTransform.position, _aimingRigTransform.transform.position);
        SendAimDirectionEvents();

    }

    private void SendAimDirectionEvents()
    {
        if (_aimingRigTransform.transform.position.x > _throwingHandTransform.position.x)
        {
            AimingRight?.Invoke();
        }
        else
        {
            AimingLeft?.Invoke();
        }
    }
    
    private void BeginAiming()
    {
        StartAiming?.Invoke();
        _aimingRig.weight = 1;
        _aiming = true;
    }

    private void EndAiming()
    {
        ReleaseAiming?.Invoke();
        _aimingRig.weight = 0;
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
        if (pickedUpInteractable.IsThrowable() && !IsHoldingObject())
        {
            PickupObject(pickedUpInteractable);
        }
    }

    public bool IsHoldingObject()
    {
        return _currentlyHoldingObject != null;
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
        float playerAndCamPosDiffZ = transform.position.z - _playerCamera.transform.position.z;
        Vector3 mousePos = _playerCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * playerAndCamPosDiffZ);
        Debug.Log(mousePos);
        Debug.Log(Input.mousePosition);
        Debug.Log(playerAndCamPosDiffZ);
        return mousePos;
    }
    
    private Vector3 GetThrowDirection()
    {
        
        Vector3 direction = ( GetMousePos() - _currentlyHoldingObject.transform.position)
            .normalized;
        direction = new Vector3(direction.x, direction.y, 0);
        return direction;
    }
}
