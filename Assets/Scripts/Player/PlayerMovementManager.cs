using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    Rigidbody _rb;

    Vector3 _targetVelocity;
    public Vector3 TargetVelocity
    {
        get => _targetVelocity;
        set => _targetVelocity = value;
    }

    float _lerpRate;
    public float LerpRate
    {
        get => _lerpRate;
        set => _lerpRate = value;
    }

    Vector3 _gravityAcceleration;
    public Vector3 GravityAcceleration
    {
        get => _gravityAcceleration;
        set => _gravityAcceleration = value;
    }

    public Vector3 GetVelocity()
    {
        return _rb.velocity;
    }

    void Awake()
    {
        TryGetComponent(out _rb);
    }

    public void ApplyGravity()
    {
        _rb.AddForce(_gravityAcceleration, ForceMode.Acceleration);
    }

    public void ApplyVelocityChange()
    {
        _rb.velocity = Utilities.FRILerp(_rb.velocity, _targetVelocity, _lerpRate, Time.fixedDeltaTime);
        //_rb.AddForce(Utilities.FRILerp(_rb.velocity, _targetVelocity, _lerpRate, Time.fixedDeltaTime), ForceMode.VelocityChange);
    }

    public void AddForce(Vector3 val, ForceMode mode)
    {
        _rb.AddForce(val, mode);
    }

}
