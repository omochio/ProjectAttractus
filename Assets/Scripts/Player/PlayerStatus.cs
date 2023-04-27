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
}