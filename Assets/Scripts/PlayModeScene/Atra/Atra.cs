using UnityEngine;

public class Atra : MonoBehaviour, IAtra
{
    [SerializeField]
    AtraParameter _atraParameter;

    Rigidbody _rb;

    float _elapsedTime;
    

    void Awake()
    {
        TryGetComponent(out _rb);
        _rb.velocity = transform.rotation * (_atraParameter.Speed * Vector3.forward);
        _rb.mass = _atraParameter.Mass;
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _atraParameter.LifeTime)
        {
            Destroy(gameObject);
        }

    }

    public void AddAtraForce(Transform playerTransform, Rigidbody playerRb)
    {
        Vector3 dif = transform.position - playerTransform.position;
        float distance = dif.magnitude;
        if (distance >= _atraParameter.ForceValidDistance)
        {
            Vector3 force = dif.normalized * (_rb.mass * playerRb.mass / distance);
            playerRb.AddForce(force, ForceMode.Force);
        }
    }
}