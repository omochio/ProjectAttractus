using UnityEngine;

public class AtraGun : MonoBehaviour, IAtraGun
{
    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    GameObject _atraPrefab;

    GameObject _atraObj;
    IAtra _atra;

    public void Shot()
    {
        if (!_atraObj)
        {
            _atraObj = Instantiate(_atraPrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        }
        _playerStatus.IsAtraGunHanded = false;
        _playerStatus.IsWeaponHanded = true;
        _playerStatus.AttackInvoked = false;
    }

    public void AddAtraForce(Transform playerTransform, Rigidbody playerRb)
    {
        if (_atraObj)
        {
            _atraObj.TryGetComponent(out _atra);
            _atra.AddAtraForce(playerTransform, playerRb);
        }
        else
        {
            _playerStatus.IsAtraForceEnabled = false;
        }
    }
}
