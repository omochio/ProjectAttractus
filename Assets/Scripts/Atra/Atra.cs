using Unity.Mathematics;
using UnityEngine;

public class Atra : MonoBehaviour, IAtra
{
    [SerializeField]
    float _speed;

    [SerializeField]
    float _lifeTime;

    Rigidbody _rb;

    float _elapsedTime;
    //Vector3 _beginPos;


    void Awake()
    {
        TryGetComponent(out _rb);
        //_beginPos = transform.position;
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

    public void AddAtraForce(Transform playerTransform, PlayerMovementManager playerMovementManager)
    {
        Vector3 dif = transform.position - playerTransform.position;
        float distance = dif.magnitude;
        Vector3 force = dif.normalized * (_rb.mass * playerMovementManager.GetMass() / distance);
        playerMovementManager.AddForce(force, ForceMode.Force);
    }

    //void FixedUpdate()
    //{
    //    //if ((transform.position - _beginPos).sqrMagnitude < Mathf.Pow(_distance, 2f))
    //    //{
    //    //    transform.position += transform.rotation * (_speed * Time.fixedDeltaTime * Vector3.forward);
    //    //}
    //    if ((transform.position - _beginPos).sqrMagnitude >= Mathf.Pow(_distance, 2f))
    //    {
    //        _rb.velocity = Vector3.zero;
    //    }
    //}

}