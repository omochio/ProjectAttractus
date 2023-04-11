using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    readonly float _damage;
    [SerializeField]
    readonly float _fireRate;
    [SerializeField]
    readonly int _magazineSize;
    [SerializeField]
    public readonly bool _isAutomatic;
    [SerializeField]
    readonly float _reloadTime;

    float _elapsedTime;
    float _bulletsCount;

    PlayerStatuses _playerStatuses;
    

    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        _bulletsCount = _magazineSize;
    }

    public void InitElapsedTime()
    {
        _elapsedTime = 0f;
    }

    public void Attack()
    {
        _elapsedTime += Time.fixedDeltaTime;

        if (_bulletsCount == 0)
        {
            _playerStatuses.reloadInvoked = true;
            _playerStatuses.attackInvoked = false;
        }
        else
        {
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            if (_isAutomatic)
            {
                if (_elapsedTime % (1f / _fireRate) == 0)
                {
                    --_bulletsCount;
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
            else
            {
                --_bulletsCount;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                _playerStatuses.attackInvoked = false;
            }
        }
    }

    public void Reload() 
    {
        _elapsedTime += Time.fixedDeltaTime;

        if (_elapsedTime >= _reloadTime)
        {
            _bulletsCount = _magazineSize;
            _playerStatuses.reloadInvoked = false;
        }
    }
}
