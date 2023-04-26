using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [NonSerialized]
    public bool moveInvoked = false;
    [NonSerialized]
    public bool sprintInvoked = false;
    [NonSerialized]
    public bool jumpInvoked = false;
    [NonSerialized]
    public bool crouchOrSlideInvoked = false;
    [NonSerialized]
    public bool attackInvoked = false;
    [NonSerialized]
    public bool reloadInvoked = false;

    [NonSerialized]
    public bool isAlive = true;

    [NonSerialized]
    public bool isGrounded = true;
    [NonSerialized]
    public bool isSlidable = false;
    [NonSerialized]
    public bool isSlideCooling = false;
    [NonSerialized]
    public bool isAtraForceEnabled = false;

    [NonSerialized]
    public bool isWeaponHanded = true;
    [NonSerialized]
    public bool isAtraGunHanded = false;
        
    [NonSerialized]
    float _slideElapsedTime;
    public float SlideElapsedTime
    {
        get { return _slideElapsedTime; }
        set { _slideElapsedTime = value; }
    }
}