using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayInputManager : MonoBehaviour
{
    PlayerInput _input;
    [SerializeField]
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
                _playerStatus.MoveInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _moveInput = Vector2.zero;
                _playerStatus.MoveInvoked = false;
                break;
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.SprintInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.SprintInvoked = false;
                break;
        }
    }

    void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.IsAtraForceEnabled = false;
                if (_playerStatus.IsGrounded)
                {
                    _playerStatus.JumpInvoked = true;
                }
                break;
        }
    }

    void OnCrouchOrSlide(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.CrouchOrSlideInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.CrouchOrSlideInvoked = false;
                break;
        }
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.AttackInvoked = true;
                break;
            case InputActionPhase.Canceled:
                _playerStatus.AttackInvoked = false;
                break;
        }
    }

    void OnReload(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.ReloadInvoked = true;
                break;
        }
    }

    void OnSwitchToAtraGunHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.IsWeaponHanded = false;
                _playerStatus.IsAtraGunHanded = true;
                break;
        }
    }

    void OnSwitchToWeaponHolder(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerStatus.IsWeaponHanded = true;
                _playerStatus.IsAtraGunHanded = false;
                break;
        }
    }

    void OnEnableAtraForce(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                if (_playerStatus.IsAtraForceEnabled)
                {
                    _playerStatus.IsAtraForceEnabled = false;
                }
                else
                {
                    _playerStatus.IsAtraForceEnabled = true;
                }
                break;
        }
    }
}
