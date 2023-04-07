using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    PlayerInput _input;
    PlayerStatuses _playerStatuses;

    Vector2 _moveInput = new();
    public Vector2 moveInput
    {
        get { return _moveInput; }
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
}
