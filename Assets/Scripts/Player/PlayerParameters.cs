using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    [SerializeField]
    Vector2 _walkSpeed;
    public Vector2 WalkSpeed
    {
        get { return _walkSpeed; }
    }

    [SerializeField]
    Vector2 _sprintSpeed;
    public Vector2 SprintSpeed
    {
        get { return _sprintSpeed; }
    }

    [SerializeField]
    Vector2 _crouchSpeed;
    public Vector2 CrouchSpeed
    {
        get { return _crouchSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _slideForce;
    public float SlideForce
    {
        get { return _slideForce; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _smallSlideForce;
    public float SmallSlideForce
    {
        get { return _smallSlideForce; }
    }

    [SerializeField]
    Vector2 _slideResistanceAcceleration;
    public Vector2 SlideResistanceAcceleration
    {
        get { return _slideResistanceAcceleration; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _minSlidableSpeed;
    public float MinSlidableSpeed
    {
        get { return _minSlidableSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _slideCoolTime;
    public float SlideCoolTime
    {
        get { return _slideCoolTime; }
    }
    

    [SerializeField, Range(0f, float.MaxValue)]
    float _jumpForce;
    public float JumpForce
    {
        get { return _jumpForce; }
    }

    [SerializeField]
    Vector2 _jumpAdditionalSpeed;
    public Vector2 JumpAdditionalSpeed
    {
        get { return _jumpAdditionalSpeed; }
    }

    [SerializeField]
    Vector2 _AtraForceSpeed;
    public Vector2 AtraForceSpeed
    {
        get { return _AtraForceSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _gravityAcceleration;
    public float GravityAcceleration
    {
        get { return _gravityAcceleration; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _basicSpeedLerpRate;
    public float BasicSpeedLerpRate
    {
        get { return _basicSpeedLerpRate; }
    }
}
