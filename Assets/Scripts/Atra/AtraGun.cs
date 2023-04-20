using UnityEngine;

public class AtraGun : MonoBehaviour, IAtraGun
{
    [SerializeField]
    Transform _muzzlePos;

    [SerializeField]
    PlayerStatuses _playerStatuses;

    [SerializeField]
    PlayerMovementManager _playerMovementManager;

    [SerializeField]
    GameObject _atraPrefab;

    GameObject _atraObj;

    public void Shot()
    {
        if (!_atraObj)
        {
            _atraObj = Instantiate(_atraPrefab, _muzzlePos.position, Camera.main.transform.rotation);
        }
        _playerStatuses.isAtraGunHanded = false;
        _playerStatuses.isWeaponHanded = true;
        _playerStatuses.attackInvoked = false;
    }

    public void AddAtraForce(Transform playerTransform)
    {
        if (_atraObj)
        {
            Quaternion.LookRotation
        }
    }
}
