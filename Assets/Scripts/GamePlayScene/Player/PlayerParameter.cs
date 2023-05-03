using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Parameter/PlayerParameter")]
public class PlayerParameter : ScriptableObject
{
    [SerializeField, Range(0f, float.MaxValue)]
    float _mass;
    public float Mass
    {
        get { return _mass; }
    }

    [SerializeField]
    Vector2 _walkSpeed;
    public Vector3 WalkSpeed
    {
        get { return new Vector3(_walkSpeed.x, 0f, _walkSpeed.y); }
    }

    [SerializeField]
    Vector2 _sprintSpeed;
    public Vector3 SprintSpeed
    {
        get { return new Vector3(_sprintSpeed.x, 0f, _sprintSpeed.y); }
    }

    [SerializeField]
    Vector2 _crouchSpeed;
    public Vector3 CrouchSpeed
    {
        get { return new Vector3(_crouchSpeed.x, 0f, _crouchSpeed.y); }
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
    public Vector3 SlideResistanceAcceleration
    {
        get { return new Vector3(_slideResistanceAcceleration.x, 0f, _slideResistanceAcceleration.y); }
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
    Vector2 _jumpHorizontalAcceleration;
    public Vector3 JumpHorizontalAcceleration
    {
        get { return new Vector3(_jumpHorizontalAcceleration.x, 0f, _jumpHorizontalAcceleration.y); }
    }

    [SerializeField]
    Vector2 _atraForceHorizontalAcceleration;
    public Vector3 AtraForceHorizontalAcceleration
    {
        get { return new Vector3(_atraForceHorizontalAcceleration.x, 0f, _atraForceHorizontalAcceleration.y); }
    }

    [SerializeField]
    Vector2 _fallHorizontalAcceleration;
    public Vector3 FallHorizontalAcceleration
    {
        get { return new Vector3(_fallHorizontalAcceleration.x, 0f, _fallHorizontalAcceleration.y); }
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
