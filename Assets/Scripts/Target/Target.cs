using UnityEngine;

public class Target : MonoBehaviour, IShootable
{
    [SerializeField]
    TargetParameter _targetParameter;

    float _elapsedSpawnTime;
    float _rotateInterval;
    float _helth;

    bool _isBroken = false;
    public bool IsBroken
    {
        get { return _isBroken; }
        set { _isBroken = value; }
    }

    Rigidbody _rb;
    ScoreManager _scoreManager;

    public void Init(ScoreManager sm)
    {
        _scoreManager = sm;
        _helth = _targetParameter.InitHelth;
    }

    void Awake()
    {
        TryGetComponent(out _rb);
        transform.rotation = Random.rotation;
    }

    void Update()
    {
        _elapsedSpawnTime += Time.deltaTime;
        if (_elapsedSpawnTime >= _rotateInterval)
        {
            transform.rotation *= Random.rotation;
            _rotateInterval = Random.Range(_targetParameter.MinRotateInterval, _targetParameter.MaxRotateInterval);
            _elapsedSpawnTime = 0f;
        }
    }

    void FixedUpdate()
    {
        _rb.AddForce(transform.rotation *  Vector3.forward * _targetParameter.MoveSpeed, ForceMode.VelocityChange);
    }

    public void OnShot(float damage)
    {
        _helth -= damage;
        if (_helth <= 0)
        {
            _isBroken = true;
            _scoreManager.AddScore(_targetParameter.Score);
        }
    }

}
