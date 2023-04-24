using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayInputManager : MonoBehaviour
{
    PlayerInput _input;
    PlayerStatuses _playerStatuses;

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
        TryGetComponent(out _playerStatuses);
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
                _playerStatuses.moveInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _moveInput = Vector2.zero;
                _playerStatuses.moveInvoked = false;
                break;
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.sprintInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatuses.sprintInvoked = false;
                break;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.isAtraForceEnabled = false;
                if (_playerStatuses.isGrounded)
                {
                    _playerStatuses.jumpInvoked = true;
                }
                break;
        }
    }

    void OnCrouchOrSlide(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.crouchOrSlideInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatuses.crouchOrSlideInvoked = false;
                break;
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.attackInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatuses.attackInvoked = false;
                break;
        }
    }

    void OnReload(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.reloadInvoked = true;
                break;
        }
    }

    void OnSwitchToAtraGunHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.isWeaponHanded = false;
                _playerStatuses.isAtraGunHanded = true;
                break;
        }
    }

    void OnSwitchToWeaponHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatuses.isWeaponHanded = true;
                _playerStatuses.isAtraGunHanded = false;
                break;
        }
    }

    void OnEnableAtraForce(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (_playerStatuses.isAtraForceEnabled)
                {
                    _playerStatuses.isAtraForceEnabled = false;
                }
                else
                {
                    _playerStatuses.isAtraForceEnabled = true;
                }
                break;
        }
    }
}
