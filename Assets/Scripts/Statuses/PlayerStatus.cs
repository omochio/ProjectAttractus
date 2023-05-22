using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    Vector2 _moveInput = new();
    public Vector3 MoveInput
    {
        get { return new Vector3(_moveInput.x, 0f, _moveInput.y); }
        set { _moveInput = value; }
    }

    Vector3 _smoothedMoveInput = new();
    public Vector3 SmoothedMoveInput
    {
        get { return _smoothedMoveInput; }
        set { _smoothedMoveInput = value; }
    }

    bool _moveInvoked = false;
    public bool MoveInvoked
    {
        get => _moveInvoked;
        set => _moveInvoked = value;
    }

    bool _sprintInvoked = false;
    public bool SprintInvoked
    {
        get => _sprintInvoked;
        set => _sprintInvoked = value;
    }

    bool _jumpInvoked = false;
    public bool JumpInvoked
    {
        get => _jumpInvoked;
        set => _jumpInvoked = value;
    }

    bool _crouchOrSlideInvoked = false;
    public bool CrouchOrSlideInvoked
    {
        get => _crouchOrSlideInvoked;
        set => _crouchOrSlideInvoked = value;
    }

    bool _attackInvoked = false;
    public bool AttackInvoked
    {
        get => _attackInvoked; 
        set => _attackInvoked = value;
    }

    bool _reloadInvoked = false;
    public bool ReloadInvoked
    {
        get => _reloadInvoked;
        set => _reloadInvoked = value;
    }

    bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set => _isAlive = value;
    }

    bool _isGrounded = true;
    public bool IsGrounded
    {
        get => _isGrounded;
        set => _isGrounded = value;
    }

    bool _isSlidable = false;
    public bool IsSlidable
    {
        get => _isSlidable;
        set => _isSlidable = value;
    }

    bool _isSlideCooling = false;
    public bool IsSlideCooling
    {
        get => _isSlideCooling;
        set => _isSlideCooling = value;
    }

    bool _isAtraForceEnabled = false;
    public bool IsAtraForceEnabled
    {
        get => _isAtraForceEnabled; 
        set => _isAtraForceEnabled = value;
    }

    bool _isWeaponHanded = true;
    public bool IsWeaponHanded
    {
        get => _isWeaponHanded; 
        set => _isWeaponHanded = value;
    }

    bool _isAtraGunHanded = false;
    public bool IsAtraGunHanded
    {
        get => _isAtraGunHanded; 
        set => _isAtraGunHanded = value;
    }

    float _slideElapsedTime = 0f;
    public float SlideElapsedTime
    {
        get { return _slideElapsedTime; }
        set { _slideElapsedTime = value; }
    }

    bool _isGravityEnabled = true;
    public bool IsGravityEnabled
    {
        get => _isGravityEnabled;
        set => _isGravityEnabled = value;
    }
}