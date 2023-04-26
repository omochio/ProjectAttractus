using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    WeaponParameter _weaponParameter;

    float _reloadElapsedTime;
    float _shotElapsedTime;
    float _bulletsCount;

    PlayerStatus _playerStatus;
    
    public void Init(PlayerStatus ps)
    {
        _playerStatus = ps;
    }

    void Awake()
    {
        _bulletsCount = _weaponParameter.MagazineSize;
    }

    public void ResetTimeCount()
    {
        _shotElapsedTime = (1f / _weaponParameter.FireRate);
        _reloadElapsedTime = 0f;
    }

    public void Shot()
    {
        _shotElapsedTime += Time.fixedDeltaTime;

        if (_bulletsCount == 0)
        {
            _playerStatus.reloadInvoked = true;
        }
        else
        {
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            //Debug.Log(_bulletsCount);
            if (_weaponParameter.IsAutomatic)
            {
                if (_shotElapsedTime >= (1f / _weaponParameter.FireRate))
                {
                    --_bulletsCount;
                    // TODO: Temporary implementation
                    Instantiate(_weaponParameter.ProjectileObj, Camera.main.transform.position, Camera.main.transform.rotation);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.gameObject.TryGetComponent(out IShootable shootable))
                        {
                            shootable.OnShot(_weaponParameter.Damage);
                        }
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
                _playerStatus.attackInvoked = false;
            }
        }
    }

    public void Reload() 
    {
        _reloadElapsedTime += Time.fixedDeltaTime;

        if (_reloadElapsedTime >= _weaponParameter.ReloadTime)
        {
            _bulletsCount = _weaponParameter.MagazineSize;
            _playerStatus.reloadInvoked = false;
            _reloadElapsedTime = 0f;
        }
    }
}
