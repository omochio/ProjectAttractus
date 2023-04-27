using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Status/PlayerStatus")]
public class PlayerStatus : ScriptableObject
{
    void OnEnable()
    {
        MoveInvoked = _moveInvoked;
        SprintInvoked = _sprintInvoked;
        JumpInvoked = _jumpInvoked;
        CrouchOrSlideInvoked = _crouchOrSlideInvoked;
        AttackInvoked = _attackInvoked;
        ReloadInvoked = _reloadInvoked;
        IsAlive = _isAlive;
        IsGrounded = _isGrounded;
        IsSlidable = _isSlidable;
        IsSlideCooling = _isSlideCooling;
        IsAtraForceEnabled = _isAtraForceEnabled;
        IsWeaponHanded = _isWeaponHanded;
        IsAtraGunHanded = _isAtraGunHanded;
        SlideElapsedTime = _slideElapsedTime;
    }

    [NonSerialized]
    bool _moveInvoked = false;
    public bool MoveInvoked
    {
        get => _moveInvoked;
        set => _moveInvoked = value;
    }

    [NonSerialized]
    bool _sprintInvoked = false;
    public bool SprintInvoked
    {
        get => _sprintInvoked;
        set => _sprintInvoked = value;
    }

    [NonSerialized]
    bool _jumpInvoked = false;
    public bool JumpInvoked
    {
        get => _jumpInvoked;
        set => _jumpInvoked = value;
    }

    [NonSerialized]
    bool _crouchOrSlideInvoked = false;
    public bool CrouchOrSlideInvoked
    {
        get => _crouchOrSlideInvoked;
        set => _crouchOrSlideInvoked = value;
    }

    [NonSerialized]
    bool _attackInvoked = false;
    public bool AttackInvoked
    {
        get => _attackInvoked; 
        set => _attackInvoked = value;
    }

    [NonSerialized]
    bool _reloadInvoked = false;
    public bool ReloadInvoked
    {
        get => _reloadInvoked;
        set => _reloadInvoked = value;
    }

    [NonSerialized]
    bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set => _isAlive = value;
    }

    [NonSerialized]
    bool _isGrounded = true;
    public bool IsGrounded
    {
        get => _isGrounded;
        set => _isGrounded = value;
    }

    [NonSerialized]
    bool _isSlidable = false;
    public bool IsSlidable
    {
        get => _isSlidable;
        set => _isSlidable = value;
    }

    [NonSerialized]
    bool _isSlideCooling = false;
    public bool IsSlideCooling
    {
        get => _isSlideCooling;
        set => _isSlideCooling = value;
    }

    [NonSerialized]
    bool _isAtraForceEnabled = false;
    public bool IsAtraForceEnabled
    {
        get => _isAtraForceEnabled; 
        set => _isAtraForceEnabled = value;
    }

    [NonSerialized]
    bool _isWeaponHanded = true;
    public bool IsWeaponHanded
    {
        get => _isWeaponHanded; 
        set => _isWeaponHanded = value;
    }

    [NonSerialized]
    bool _isAtraGunHanded = false;
    public bool IsAtraGunHanded
    {
        get => _isAtraGunHanded; 
        set => _isAtraGunHanded = value;
    }

    [NonSerialized]
    float _slideElapsedTime = 0f;
    public float SlideElapsedTime
    {
        get { return _slideElapsedTime; }
        set { _slideElapsedTime = value; }
    }
}