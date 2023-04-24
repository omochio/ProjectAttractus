using UnityEngine;

public class Atra : MonoBehaviour, IAtra
{
    [SerializeField]
    float _speed;

    [SerializeField]
    float _lifeTime;

    [SerializeField]
    float _forceEnableDistance;

    Rigidbody _rb;

    float _elapsedTime;
    

    void Awake()
    {
        TryGetComponent(out _rb);
    }

    void Start()
    {
        _rb.velocity = transform.rotation * (_speed * Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _lifeTime)
        {
            Destroy(gameObject);
        }

    }

    public void AddAtraForce(Transform playerTransform, Rigidbody playerRb)
    {
        Vector3 dif = transform.position - playerTransform.position;
        float distance = dif.magnitude;
        if (distance >= _forceEnableDistance)
        {
            Vector3 force = dif.normalized * (_rb.mass * playerRb.mass / distance);
            playerRb.AddForce(force, ForceMode.Force);
        }
    }
}