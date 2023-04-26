using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField]
    WeaponParameter _weaponParameter;

    TMP_Text _ammoText;

    float _reloadElapsedTime;
    float _shotElapsedTime;
    float _ammoCount;

    PlayerStatus _playerStatus;
    
    public void Init(PlayerStatus ps, TMP_Text txt)
    {
        _playerStatus = ps;
        _ammoText = txt;

        _ammoCount = _weaponParameter.MagazineSize;
        _ammoText.text = $"Ammo: {_ammoCount}";
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
                    --_ammoCount;
                    _ammoText.text = $"Ammo: {_ammoCount}";
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
                _ammoText.text = $"Ammo: {_ammoCount}";
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
        _ammoText.text = $"Ammo: <color=#FFFF00FF>Reloading</color>";

        if (_reloadElapsedTime >= _weaponParameter.ReloadTime)
        {
            _ammoCount = _weaponParameter.MagazineSize;
            _ammoText.text = $"Ammo: {_ammoCount}";
            _playerStatus.reloadInvoked = false;
            _reloadElapsedTime = 0f;
        }
    }
}
