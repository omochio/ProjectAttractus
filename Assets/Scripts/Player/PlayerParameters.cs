using UnityEngine;

public class PlayerParameters : MonoBehaviour
{
    [SerializeField]
    Vector2 _walkSpeed;
    public Vector2 walkSpeed
    {
        get { return _walkSpeed; }
    }

    [SerializeField]
    Vector2 _sprintSpeed;
    public Vector2 sprintSpeed
    {
        get { return _sprintSpeed; }
    }

    [SerializeField]
    Vector2 _crouchSpeed;
    public Vector2 crouchSpeed
    {
        get { return _crouchSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _slideForce;
    public float slideForce
    {
        get { return _slideForce; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _smallSlideForce;
    public float smallSlideForce
    {
        get { return _smallSlideForce; }
    }

    [SerializeField]
    Vector2 _slideResistanceAcceleration;
    public Vector2 slideResistanceAcceleration
    {
        get { return _slideResistanceAcceleration; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _minSlidableSpeed;
    public float minSlidableSpeed
    {
        get { return _minSlidableSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _slideCoolTime;
    public float slideCoolTime
    {
        get { return _slideCoolTime; }
    }
    

    [SerializeField, Range(0f, float.MaxValue)]
    float _jumpForce;
    public float jumpForce
    {
        get { return _jumpForce; }
    }

    [SerializeField]
    Vector2 _jumpAdditionalSpeed;
    public Vector2 jumpAdditionalSpeed
    {
        get { return _jumpAdditionalSpeed; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _gravityAcceleration;
    public float gravityAcceleration
    {
        get { return _gravityAcceleration; }
    }

    [SerializeField, Range(0f, float.MaxValue)]
    float _baseSpeedLerpRate;
    public float baseSpeedLerpRate
    {
        get { return _baseSpeedLerpRate; }
    }
}
