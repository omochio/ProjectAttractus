using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{

    [SerializeField]
    float _damage;
    [SerializeField]
    float _fireRate;
    [SerializeField]
    int _magazineSize;
    [SerializeField]
    bool _isAutomatic;
    [SerializeField]
    float _reloadTime;
    [SerializeField]
    GameObject _projectileObj;
    [SerializeField]
    Transform _muzzlePos;

    float _reloadElapsedTime;
    float _shotElapsedTime;
    float _bulletsCount;

    // TODO: Reconsider initialize purpose
    [SerializeField]
    PlayerStatuses _playerStatuses;
    

    void Awake()
    {
        _bulletsCount = _magazineSize;
    }

    public void InitTimeCount()
    {
        _shotElapsedTime = (1f / _fireRate);
        _reloadElapsedTime = 0f;
    }

    public void Shot()
    {
        _shotElapsedTime += Time.fixedDeltaTime;

        if (_bulletsCount == 0)
        {
            _playerStatuses.reloadInvoked = true;
            //_playerStatuses.attackInvoked = false;
        }
        else
        {
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.Log(_bulletsCount);
            if (_isAutomatic)
            {
                if (_shotElapsedTime >= (1f / _fireRate))
                {
                    --_bulletsCount;
                    // TODO: Temporary implementation
                    Instantiate(_projectileObj, _muzzlePos.position, Camera.main.transform.rotation);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    _shotElapsedTime = 0f;
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
        _reloadElapsedTime += Time.fixedDeltaTime;

        if (_reloadElapsedTime >= _reloadTime)
        {
            _bulletsCount = _magazineSize;
            _playerStatuses.reloadInvoked = false;
            _reloadElapsedTime = 0f;
        }
    }
}
