using System;
using System.Collections;
using UnityEngine;
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _modelHolder;

    private bool _facingRight;
    private bool _currentlyTurning;
    private Coroutine _turningCoroutine;

    
    private static readonly int RunningHash = Animator.StringToHash("running");
    private static readonly int WalkingHash = Animator.StringToHash("walking");
    private static readonly int JumpHash = Animator.StringToHash("jump");
    private static readonly int GroundedHash = Animator.StringToHash("grounded");
    private static readonly int TurnHash = Animator.StringToHash("turn");

    private static readonly float _turntime = 0.2f;

    
    [SerializeField] private MovementType _movementType;
    private static readonly int PickupHash = Animator.StringToHash("pickup");
    private static readonly int ThrowstanceHash = Animator.StringToHash("throwstance");
    private static readonly int PropertyHash = Animator.StringToHash("throw");

    public static event Action PickupAnimationStarted;
    public static event Action PickupAnimationEnded;

    [Serializable]
    private enum MovementType
    {
        Walking,
        Running
    }
    
    private void OnEnable()
    {
        
        PlayerController.StartedMovingRight += StartHorizontalMovement;
        PlayerController.StartedMovingRight += SetFacingRight;
        PlayerController.StartedMovingRight += FlipModelHolderRight;
        
        PlayerController.StartedMovingLeft += StartHorizontalMovement;
        PlayerController.StartedMovingLeft += SetFacingLeft;
        PlayerController.StartedMovingLeft += FlipModelHolderLeft;
        
        PlayerController.NotMoving += StopHorizontalMovement;
        
        PlayerController.StartedJump += StartJump;
        GroundedChecker.OnLandedOnGround += EndJump;

        PickupInteractable.PickupCollected += StartPickupInteractable;

        PlayerObjectThrowing.StartAiming += StartAimingStance;
        PlayerObjectThrowing.ReleaseAiming += StartThrowAnim;
        PlayerObjectThrowing.AimingRight += FlipModelHolderRight;
        PlayerObjectThrowing.AimingLeft += FlipModelHolderLeft;

        SneakArea.PlayerEnteredSneakArea += SneakPlayer;
        SneakArea.PlayerLeftSneakArea += UnSneakPlayer;
    }

    private void OnDisable()
    {
        PlayerController.StartedMovingRight -= StartHorizontalMovement;
        PlayerController.StartedMovingRight -= SetFacingRight;
        PlayerController.StartedMovingRight -= FlipModelHolderRight;
        PlayerController.StartedMovingLeft -= StartHorizontalMovement;
        PlayerController.StartedMovingRight -= SetFacingLeft;
        PlayerController.StartedMovingLeft -= FlipModelHolderLeft;
        
        PlayerController.NotMoving -= StopHorizontalMovement;
        
        PlayerController.StartedJump -= StartJump; 
        GroundedChecker.OnLandedOnGround -= EndJump;
        
        PickupInteractable.PickupCollected -= StartPickupInteractable;
        
        PlayerObjectThrowing.StartAiming -= StartAimingStance;
        PlayerObjectThrowing.ReleaseAiming -= StartThrowAnim;
        PlayerObjectThrowing.AimingRight -= FlipModelHolderRight;
        PlayerObjectThrowing.AimingLeft -= FlipModelHolderLeft;
        
        SneakArea.PlayerEnteredSneakArea -= SneakPlayer;
        SneakArea.PlayerLeftSneakArea -= UnSneakPlayer;
    }

    private void StartAimingStance()
    {
        _animator.SetBool(ThrowstanceHash, true);
    }

    private void StartThrowAnim()
    {
        _animator.SetBool(ThrowstanceHash, false);
        _animator.SetTrigger(PropertyHash);
    }
    
    private void FlipModelHolderRight()
    {
        _modelHolder.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void FlipModelHolderLeft()
    {
        _modelHolder.transform.rotation = Quaternion.Euler(0, -90, 0);
    }
    
    private void SetFacingRight()
    {
        _facingRight = true;
        
    }

    private void SetFacingLeft()
    {
        _facingRight = false;
    }
    
    /* TODO: Maybe implement this but too much brain rot right now lol rofl xdd
     
     I'm deeply sorry Martin Robert C. for leaving commented code in here but this time it really makes sense <3
     
     private IEnumerator RotateModel()
    {
        var targetRotation = _facingRight ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);
        var currentRotation = _modelHolder.transform.rotation;
        float  time = 0f;
        while (time < _turntime)
        {
            time += Time.deltaTime;
            _modelHolder.transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, time);
            yield return null;
        }
    }
    
    private void TurnPlayerInMovementDirection()
    {
        _animator.SetTrigger(TurnHash);
        _turningCoroutine = StartCoroutine(RotateModel());
    }*/
    
    private void StartJump()
    {
        _animator.SetTrigger(JumpHash);
        
    }

    private void EndJump()
    {
        _animator.SetTrigger(GroundedHash);
    }
    
    private void StartHorizontalMovement()
    {

        if (IsMovementTypeRunning())
        {
            StartRunning();
        }

        else if (IsMovementTypeWalking())
        {
            StartWalking();
        }
    }
    
    private void StopHorizontalMovement()
    {
        if (IsMovementTypeRunning())
        {
            StopRunning();
        }

        else if (IsMovementTypeWalking())
        {
            StopWalking();
        }
    }

    private void StartRunning()
    {
        _animator.SetBool(RunningHash, true);
    }

    private void StopRunning()
    {
        _animator.SetBool(RunningHash, false);
    }

    private void StartWalking()
    {
        _animator.SetBool(WalkingHash, true);
    }
    
    private void StopWalking()
    {
        _animator.SetBool(WalkingHash, false);
    }

    private bool IsMovementTypeRunning()
    {
        return _movementType == MovementType.Running;
    }

    private bool IsMovementTypeWalking()
    {
        return _movementType == MovementType.Walking;
    }

    private void StartPickupInteractable(PickupInteractable pickup = null)
    {
        _animator.SetTrigger(PickupHash);
        PickupAnimationStarted?.Invoke();
    }

    private void SneakPlayer()
    {
        _animator.SetFloat("sneak", 1);
    }

    private void UnSneakPlayer()
    {
        _animator.SetFloat("sneak", 0);
    }
}
