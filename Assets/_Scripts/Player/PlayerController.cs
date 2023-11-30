using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private KeyCode _moveKeyLeft;
    private KeyCode _moveKeyRight;
    private KeyCode _moveKeyJump;
    private KeyCode _interactKey;
    
    public static PlayerController Instance;

    public event Action playerDied;

    private Animator _anim;
    private Rigidbody _rigidbody;
    [SerializeField] private GroundedChecker _groundedChecker;
    [SerializeField] private PlayerInteraction _playerInteraction;
    [SerializeField] public PLAYERDATA Data;
    [SerializeField] private Transform _jumpEffectPosition;
    [SerializeField] private ParticleSystem _walkingParticleObject1;
    [SerializeField] private ParticleSystem _walkingParticleObject2;

    [SerializeField] private PlayerGravity _playerGravity;
    // Timestamps
    private float _jumpKeyPressedTime;
    private float _jumpKeyReleasedTime;
    private float _jumpKeyHoldTime;
    private float _lastJumpTime;
    private float _landedOnGroundTime;
    private float _leftGroundTime;
    private float _lastWallJumpTime;
    private float _wallHangTime;
    private float _lastDashTime;

    private float _remainingMovementBlockTime;
    
    private bool _didDoubleJump;

    private bool _hangingOnWall;
    private bool _wallHangBlocked;
    private bool _wallHangCountingStarted;
    private bool _lastMoveDirectionWasRight;
    private float _wallHangVelocityOverride = 1f;

    private float _dashMaxVelocityModifier = 1;
    private bool _dashing;
    
    private bool _blockVelocityChanges;

    private float _currentWallJumpAccelerationModifier = 1;
    private float _currentWallJumpDecelerationModifier = 1;
    
    private int _currentHealth;

    private bool _alreadyJumpedThisFrame;

    private int _currentKeyBindingId;
    private PlayerState _playerState;

    private bool _timedMovementBlock;
    private bool _movementBlock;
    
    // Movement events
    public static event Action StartedMovingLeft;
    public static event Action StartedMovingRight;

    public static event Action Moving;

    public static event Action NotMoving;
    public static event Action StartedJump;
    public static event Action EndedJump;
    
    // Sound Controller
    private static PlayerSoundController soundController;

    
    #region Unity Events
    
    void Update()
    {
        UpdateTimedMovementBlock();
        
        TryMove();
        SetPlayerState();
        VisualEffects();

        SendStateEvents();

        InteractionInput();
        
        SwitchControlInput();
    }

    private void SendStateEvents()
    {
        SendPlayerNotMovingEvent();
        
    }

    private void OnEnable()
    {
        GroundedChecker.OnLandedOnGround += JumpBuffering;
        GroundedChecker.OnLandedOnGround += SetLandOnGroundTime;
        GroundedChecker.OnLandedOnGround += SendLandedOnGroundEvent;
        GroundedChecker.OnLandedOnGround += SetPlayerStateWalking;
        GroundedChecker.OnLandedOnGround += SetLandedMovementBlockTime;
        GroundedChecker.OnLeftGround     += SetLeftGroundTime;

        PlayerAnimationController.PickupAnimationStarted += SetPickupMovementBlockTime;

        PlayerObjectThrowing.StartAiming += BlockMovement;
        PlayerObjectThrowing.ReleaseAiming += UnBlockMovement;
    }

    private void OnDisable()
    {
        GroundedChecker.OnLandedOnGround -= JumpBuffering;
        GroundedChecker.OnLandedOnGround -= SetLandOnGroundTime;
        GroundedChecker.OnLandedOnGround -= SendLandedOnGroundEvent;
        GroundedChecker.OnLandedOnGround -= SetPlayerStateWalking;
        GroundedChecker.OnLandedOnGround -= SetLandedMovementBlockTime;
        GroundedChecker.OnLeftGround     -= SetLeftGroundTime;
        
        PlayerAnimationController.PickupAnimationStarted -= SetPickupMovementBlockTime;
        
        PlayerObjectThrowing.StartAiming -= BlockMovement;
        PlayerObjectThrowing.ReleaseAiming -= UnBlockMovement;
    }

    private void Start()
    {
        SetupKeyBindings(_currentKeyBindingId);
        soundController = this.GetComponent<PlayerSoundController>();
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this.gameObject); 
        } 
        else 
        { 
            Instance = this; 
        }    
    }

    #endregion

    private void SetupKeyBindings(int id)
    {
        _moveKeyJump = Data.Keybindings[id].MoveKeyJump;
        _moveKeyLeft = Data.Keybindings[id].MoveKeyLeft;
        _moveKeyRight = Data.Keybindings[id].MoveKeyRight;
        _interactKey = Data.Keybindings[id].InteractKey;
        
    }

    private void SwitchControlInput()
    {
        if (Input.GetKeyDown(KeyCode.O) && ( Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
        {
            SwitchLeftRightHandedControl();
        }
    }
    
    private void SwitchLeftRightHandedControl()
    {
        if (_currentKeyBindingId == 0)
        {
            _currentKeyBindingId = 1;
            SetupKeyBindings(_currentKeyBindingId);
        }
        else
        {
            _currentKeyBindingId = 0;
            SetupKeyBindings(_currentKeyBindingId);
        }
    }
    
    #region Grounded Hanlding
    private bool IsGrounded()
    {
        return _groundedChecker.Grounded;
    }

    private void SetLandOnGroundTime()
    {
        _landedOnGroundTime = Time.time;
    }

    private void SetLandedMovementBlockTime()
    {
        SetMovementBlockForTime(Data.LandedMovementBlockTime);
    }
    
    

    private void SendLandedOnGroundEvent()
    {
        EndedJump?.Invoke();
    }

    private void SetLeftGroundTime()
    {
        _leftGroundTime = Time.time;
    }
    #endregion

    #region Gravitiy Manipulation
    private void ModifyPlayerGravityForTime(float time, float modifier)
    {
        ModifyPlayerGravity(modifier);
        Invoke(nameof(ResetPlayerGravity), time);
    }
    
    private void ModifyPlayerGravity(float modifier)
    {
        _playerGravity.SetGravityScale(modifier);
    }

    private void ResetPlayerGravity()
    {
        _playerGravity.SetGravityScale(Data.StandardGravity);
    }
    #endregion
    
    private void TryMove()
    {
        if (!IsMoveAllowed())
        {
            return;
        }
        
        HorizontalMovement();
        JumpMovement();
    }

    private bool IsMoveAllowed()
    {
        return !_timedMovementBlock && !_movementBlock;
    }

    public void BlockMovement()
    {
        SetPlayerVelocity(Vector2.zero);
        _movementBlock = true;
    }

    public void UnBlockMovement()
    {
        _movementBlock = false;
    }

    private void UpdateTimedMovementBlock()
    {
        if (_timedMovementBlock)
        {
            _remainingMovementBlockTime -= Time.deltaTime;
            if (_remainingMovementBlockTime <= 0)
            {
                _timedMovementBlock = false;
            }
        }
    }

    private void SetMovementBlockForTime(float time)
    {
        _remainingMovementBlockTime = time;
        _timedMovementBlock = true;
    }

    private void SetPickupMovementBlockTime()
    {
        SetMovementBlockForTime(Data.PickUpMovementBlockTime);
    }
    
    
    private void BlockVelocityChange()
    {
        _blockVelocityChanges = true;
    }

    private void BlockVelocityChangeForSeconds(float seconds)
    {
        BlockVelocityChange();
        Invoke(nameof(BlockVelocityChange),seconds);
    }

    private void UnBlockVelocityChange()
    {
        _blockVelocityChanges = false;
    }
    
    private bool IsPlayerVelocityChangeBlocked()
    {
        return _blockVelocityChanges;
    }
    
    private void SetPlayerVelocity(Vector2 newVelocity)
    {
        if (!IsPlayerVelocityChangeBlocked())
        {
            _rigidbody.velocity = newVelocity;
        }
    }

    private void ForceSetPlayerVelocity(Vector2 newVelocity)
    {
        _rigidbody.velocity = newVelocity;
    }
    
    #region Horizontal Movement

    private bool _isMoving;
    private bool _isFacingRight;
    private bool _flipToggled;

    private void HorizontalMovement()
    {
        MovementInputHorizontal();
        LimitMoveSpeedHorizontal();
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public bool GetFacingDirection()
    {
        return _isFacingRight;
    }
    private void MovementInputHorizontal()
    {
        if (IsMoveKeyLeftPressed())
        {
            MoveLeft();
            SetLastMoveDirectionLeft();
            StartedMovingLeft?.Invoke();
        }
        else if (IsMoveKeyRightPressed())
        {
            MoveRight();
            SetLastMoveDirectionRight();
            StartedMovingRight?.Invoke();
        }
        else
        {
            DeceleratePlayerHorizontal();
        }
    }

    private void SendPlayerNotMovingEvent()
    {
       if (IsPlayerStanding())
       {
           NotMoving?.Invoke();
       }
       else
       {
           Moving?.Invoke();
       }
    }

    private bool IsPlayerStanding()
    {
        return Mathf.Abs(_rigidbody.velocity.x) <= 0.1f;
    }

    private void SetLastMoveDirectionRight()
    {
        _lastMoveDirectionWasRight = true;
    }
    private void SetLastMoveDirectionLeft()
    {
        _lastMoveDirectionWasRight = false;
    }
    
    private bool IsMoveKeyLeftPressed()
    {
        return Input.GetKey(_moveKeyLeft);
    }

    private bool IsMoveKeyLeftReleased()
    {
        return Input.GetKeyUp(_moveKeyLeft);
    }

    private bool IsMoveKeyRightPressed()
    {
        return Input.GetKey(_moveKeyRight);
    }
    
    private bool IsMoveKeyRightReleased()
    {
        return Input.GetKeyUp(_moveKeyRight);
    }

    private void MoveLeft()
    {
        _isMoving = true;
        SetPlayerVelocity(new Vector2(_rigidbody.velocity.x - GetHorizontalAccelerationDelta() , _rigidbody.velocity.y));
    }
    
    private void MoveRight()
    {
        _isMoving = true;
        SetPlayerVelocity(new Vector2(_rigidbody.velocity.x + GetHorizontalAccelerationDelta() , _rigidbody.velocity.y));
    }

    private float GetHorizontalAccelerationDelta()
    {
        float delta = (Data.MaxVelocity.x * Data.Acceleration * Time.deltaTime) * _wallHangVelocityOverride;
        if ( ! IsGrounded())
        {
            delta *= Data.AirBornAcceleartionModifier * _currentWallJumpAccelerationModifier;
        }
        return delta;
    }
    
    private float GetHorizontalDecelerationDelta()
    {
        float delta = (Data.MaxVelocity.x * Data.Deceleration * Time.deltaTime);
        if ( ! IsGrounded())
        {
            delta *= Data.AirBornDeceleartionModifier * _currentWallJumpDecelerationModifier;
        }
        return delta;
    }

    private void DeceleratePlayerHorizontal()
    {
        Vector2 playerVelocity = _rigidbody.velocity;
        if (playerVelocity.x > 0)
        {
            playerVelocity = new Vector2(_rigidbody.velocity.x - GetHorizontalDecelerationDelta(), playerVelocity.y);
            
            if (playerVelocity.x < 0)
            {
                playerVelocity = new Vector2(0, playerVelocity.y);
            }
        }
        
        else if (playerVelocity.x < 0)
        {
            playerVelocity = new Vector2(_rigidbody.velocity.x + GetHorizontalDecelerationDelta(), playerVelocity.y);
            
            if (playerVelocity.x > 0)
            {
                playerVelocity = new Vector2(0, playerVelocity.y);
            }
        }

        SetPlayerVelocity(playerVelocity);
    }

    private void LimitMoveSpeedHorizontal()
    {
        if (_rigidbody.velocity.x > Data.MaxVelocity.x)
        {
           SetPlayerVelocity(new Vector2(Data.MaxVelocity.x * _dashMaxVelocityModifier, _rigidbody.velocity.y));
        }
        else if (_rigidbody.velocity.x < -Data.MaxVelocity.x)
        {
            SetPlayerVelocity(new Vector2(-Data.MaxVelocity.x * _dashMaxVelocityModifier, _rigidbody.velocity.y));
        }
    }
    #endregion

    #region Jump
    private void JumpMovement()
    {
        ResetAlreadyJumpedThisFrame();
        JumpInput();
        ModifyJumpGravity();
        SpeedUpFalling();
        LimitMoveSpeedVertical();
    }
    private void JumpInput()
    {
        if (Input.GetKeyDown(_moveKeyJump))
        {
            TryJump();
            SaveJumpKeyPressedTime();
        }

        if (Input.GetKeyUp(_moveKeyJump))
        {
            SaveJumpKeyReleasedTime();
        }
        
        UpdateJumpKeyHoldTime();
    }

    private void SaveJumpKeyPressedTime()
    {
        _jumpKeyPressedTime = Time.time;
    }

    private void SaveJumpKeyReleasedTime()
    {
        _jumpKeyReleasedTime = Time.time;
    }

    private void UpdateJumpKeyHoldTime()
    {
        if (_jumpKeyReleasedTime <= _jumpKeyPressedTime )
        {
            _jumpKeyHoldTime = Time.time - _jumpKeyPressedTime;
            // Limit jump key hold time to max jump key hold time
            _jumpKeyHoldTime = Mathf.Min(_jumpKeyHoldTime, Data.MaxJumpKeyHoldTime);
        }
    }
    
    private void TryJump()
    {
        if (IsJumpAllowed())
        {
            SetJumpVelocity();
            SetAlreadyJumpedThisFrame();
            SetJumpTime();
            StartedJump?.Invoke();
        }
    }

    private void SetJumpTime()
    {
        _lastJumpTime = Time.time;
    }

    private void SetJumpVelocity()
    {
        SetPlayerVelocity(new Vector2(_rigidbody.velocity.x, Data.JumpForce));
    }

    private void JumpBuffering()
    {
        if (_playerState == PlayerState.Jumping && JumpBufferingValid())
        {
            TryJump();
        }
    }

    private bool JumpBufferingValid()
    {
        if (Time.time - _jumpKeyPressedTime < Data.JumpBufferTime)
        {
            return true;
        }

        return false;
    }

    private bool IsJumpAllowed()
    {
        return ( IsGrounded() || IsKoyoteTimingAllowed()) && !DidPlayerAlreadyJumpThisFrame() && IsJumpCooldownReady();
    }

    private bool IsJumpCooldownReady()
    {
        return Time.time - _lastJumpTime > Data.JumpCooldown;
    }

    private bool IsKoyoteTimingAllowed()
    {
        if (_rigidbody.velocity.y < 0 && Time.time - _leftGroundTime < Data.KoyoteTime)
        {
            return true;
        }
        return false;
    }

    private void SpeedUpFalling()
    {
        if (IsFalling())
        {
            ModifyPlayerGravity(Data.FallingGravity);
        }
        else if (IsGrounded())
        {
            ResetPlayerGravity();
        }
    }
    
    private void ModifyJumpGravity()
    {
        if (IsPlayerJumpingUp())
        {
            ModifyPlayerGravity( CalculateJumpGravityModifier() );
        }
    }

    private float CalculateJumpGravityModifier()
    {
        return Mathf.Max(Data.MinJumpGravityModifier, Data.JumpGravityModifier * Mathf.Max(0, (1 - (_jumpKeyHoldTime * Data.JumpHoldTimeFactor / Data.MaxJumpKeyHoldTime)) ));
    }
    
    private bool IsPlayerJumpingUp()
    {
        return (_playerState == PlayerState.Jumping && _rigidbody.velocity.y > 0 && _jumpKeyHoldTime <= Data.MaxJumpKeyHoldTime);
    }
    private void LimitMoveSpeedVertical()
    {
        if (_rigidbody.velocity.y > Data.MaxVelocity.y)
        {
            SetPlayerVelocity(new Vector2(_rigidbody.velocity.x, Data.MaxVelocity.y));
        }

        if (_rigidbody.velocity.y < -Data.MaxVelocity.y)
        {
            SetPlayerVelocity(new Vector2(_rigidbody.velocity.x, - Data.MaxVelocity.y));
            soundController.LandSound();
        }
    }
    
    private bool HasPlayerReleasedJumpKey()
    {
        return _jumpKeyReleasedTime > _jumpKeyPressedTime;
    }
    
    private bool IsAtJumpPeak()
    {
        return Mathf.Abs(_rigidbody.velocity.y) < Data.JumpPeakRange;
    }

    private bool IsFalling()
    {
        return _rigidbody.velocity.y < 0 && !IsGrounded();
    }
    #endregion
    
    #region Effects

    private void VisualEffects()
    {
        ToggleWalkingParticleObject();
    }
    
    public void AddForceToPlayer(Vector2 force)
    {
        Vector2 velocity = _rigidbody.velocity;
        SetPlayerVelocity(velocity);
    }

    private void ToggleWalkingParticleObject()
    {
        return;
        // TODO: fix whatever the problem is here
        var emission = _walkingParticleObject1.emission;
        var emission2 = _walkingParticleObject2.emission;
        emission.enabled = IsGrounded();
        emission2.enabled = IsGrounded();
    }

    private void ApplyTimeFreeze(float time)
    {
        StartCoroutine(TimeFreezeCoroutine(time));
    }

    private IEnumerator TimeFreezeCoroutine(float time)
    {
        FreezeTime();
        yield return new WaitForSecondsRealtime(time);
        UnfreezeTime();
    }

    private void FreezeTime()
    {
        Time.timeScale = 0f;
    }

    private void UnfreezeTime()
    {
        Time.timeScale = 1f;
    }
    
    
    #endregion
    
    #region Playerstate
    public enum PlayerState
    {
        Walking,
        Jumping,
        Falling,
        HangingOnWall,
        Dashing,
        ResetState
    }

    private void SetPlayerStateWalking()
    {
        _playerState = PlayerState.Walking;
    }

    private void ResetPlayerState()
    {
        _playerState = PlayerState.ResetState;
    }
    
    private void SetPlayerState()
    {
        if (_playerState is PlayerState.HangingOnWall or PlayerState.Dashing)
        {
            return;
        }
        
        if (IsGrounded() && _rigidbody.velocity.y <= 0.1f )
        {
            _playerState = PlayerState.Walking;
        }

        if (!IsGrounded() && _rigidbody.velocity.y  <= 0.1f)
        {
            _playerState = PlayerState.Falling;
        }

        if (_rigidbody.velocity.y > 0.1f)
        {
            _playerState = PlayerState.Jumping;
        }
    }

    private bool DidPlayerAlreadyJumpThisFrame()
    {
        return _alreadyJumpedThisFrame;
    }
    
    private void SetAlreadyJumpedThisFrame()
    {
        _alreadyJumpedThisFrame = true;
    }

    private void ResetAlreadyJumpedThisFrame()
    {
        _alreadyJumpedThisFrame = false;
    }
    #endregion

    #region Interaction

    private bool IsInteractKeyPressed()
    {
        return Input.GetKeyDown(_interactKey);
    }
    
    private void InteractionInput()
    {
        if (IsInteractKeyPressed())
        {
            _playerInteraction.TryInteract();
        }
    }

    #endregion
   
}
