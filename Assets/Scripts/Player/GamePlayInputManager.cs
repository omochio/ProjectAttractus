using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayInputManager : MonoBehaviour
{
    PlayerInput _input;
    PlayerStatus _playerStatus;

    [SerializeField]
    PlayerParameters _playerParameters;

    Vector2 _moveInput = new();
    public Vector3 MoveInput
    {
        get { return new Vector3(_moveInput.x, 0f, _moveInput.y); }
    }

    Vector3 _smoothedMoveInput = new();
    public Vector3 SmoothedMoveInput
    {
        get { return _smoothedMoveInput; }
    }

    void Awake()
    {
        TryGetComponent(out _input);
        TryGetComponent(out _playerStatus);
    }

    void OnEnable()
    {
        _input.actions["Move"].performed += OnMove;
        _input.actions["Move"].canceled += OnMove;
        _input.actions["Sprint"].performed += OnSprint;
        _input.actions["Sprint"].canceled += OnSprint;
        _input.actions["Jump"].performed += OnJump;
        _input.actions["CrouchOrSlide"].performed += OnCrouchOrSlide;
        _input.actions["CrouchOrSlide"].canceled += OnCrouchOrSlide;
        _input.actions["Attack"].performed += OnAttack;
        _input.actions["Attack"].canceled += OnAttack;
        _input.actions["Reload"].performed += OnReload;
        _input.actions["SwitchToAtraGunHolder"].performed += OnSwitchToAtraGunHolder;
        _input.actions["SwitchToWeaponHolder"].performed += OnSwitchToWeaponHolder;
        _input.actions["EnableAtraForce"].performed += OnEnableAtraForce;
    }

    void Update()
    {
        _smoothedMoveInput = Utilities.FRILerp(_smoothedMoveInput, MoveInput, _playerParameters.BasicSpeedLerpRate, Time.deltaTime);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _moveInput = context.ReadValue<Vector2>();
                _playerStatus.moveInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _moveInput = Vector2.zero;
                _playerStatus.moveInvoked = false;
                break;
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.sprintInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.sprintInvoked = false;
                break;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.isAtraForceEnabled = false;
                if (_playerStatus.isGrounded)
                {
                    _playerStatus.jumpInvoked = true;
                }
                break;
        }
    }

    void OnCrouchOrSlide(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.crouchOrSlideInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.crouchOrSlideInvoked = false;
                break;
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.attackInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.attackInvoked = false;
                break;
        }
    }

    void OnReload(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.reloadInvoked = true;
                break;
        }
    }

    void OnSwitchToAtraGunHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.isWeaponHanded = false;
                _playerStatus.isAtraGunHanded = true;
                break;
        }
    }

    void OnSwitchToWeaponHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.isWeaponHanded = true;
                _playerStatus.isAtraGunHanded = false;
                break;
        }
    }

    void OnEnableAtraForce(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (_playerStatus.isAtraForceEnabled)
                {
                    _playerStatus.isAtraForceEnabled = false;
                }
                else
                {
                    _playerStatus.isAtraForceEnabled = true;
                }
                break;
        }
    }
}
