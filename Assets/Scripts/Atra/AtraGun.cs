using UnityEngine;

public class AtraGun : MonoBehaviour, IAtraGun
{
    [SerializeField]
    Transform _muzzlePos;

    [SerializeField]
    PlayerStatuses _playerStatuses;

    [SerializeField]
    GameObject _atraPrefab;

    GameObject _atraObj;
    IAtra _atra;

    public void Shot()
    {
        if (!_atraObj)
        {
            // TODO: Rotation is not properly
            _atraObj = Instantiate(_atraPrefab, _muzzlePos.position, Camera.main.transform.rotation);
        }
        _playerStatuses.isAtraGunHanded = false;
        _playerStatuses.isWeaponHanded = true;
        _playerStatuses.attackInvoked = false;
    }

    public void AddAtraForce(Transform playerTransform, PlayerMovementManager playerMovementManager)
    {
        //Debug.Log("Atra");
        if (_atraObj)
        {
            _atraObj.TryGetComponent(out _atra);
            _atra.AddAtraForce(playerTransform, playerMovementManager);
        }
        else
        {
            _playerStatuses.isAtraForceEnabled = false;
        }
    }

    //public void EnableAtraForce(Transform playerTransform)
    //{
    //    if (!_atraObj)
    //    {
    //        _atraObj.TryGetComponent(out _atra);
    //        _atra.EnableAtraForce(playerTransform);
    //    }
    //}
}
