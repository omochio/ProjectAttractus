using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    WeaponParameter _weaponParameter;
    [SerializeField]
    PlayerStatus _playerStatus;

    float _reloadElapsedTime;
    float _shotElapsedTime;
    int _ammoCount;
    public int AmmoCount
    {
        get => _ammoCount;
    }

    
    public void Init()
    {
        _ammoCount = _weaponParameter.MagazineSize;
    }

    public void ResetTimeCount()
    {
        _shotElapsedTime = (1f / _weaponParameter.FireRate);
        _reloadElapsedTime = 0f;
    }

    public void Shot()
    {
        _shotElapsedTime += Time.fixedDeltaTime;

        if (_ammoCount == 0)
        {
            _playerStatus.ReloadInvoked = true;
        }
        else
        {
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            if (_weaponParameter.IsAutomatic)
            {
                if (_shotElapsedTime >= (1f / _weaponParameter.FireRate))
                {
                    --_ammoCount;
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
                --_ammoCount;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                _playerStatus.AttackInvoked = false;
            }
        }
    }

    public void Reload() 
    {
        _reloadElapsedTime += Time.fixedDeltaTime;

        if (_reloadElapsedTime >= _weaponParameter.ReloadTime)
        {
            _ammoCount = _weaponParameter.MagazineSize;
            _playerStatus.ReloadInvoked = false;
            _reloadElapsedTime = 0f;
        }
    }
}
