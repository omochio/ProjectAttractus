﻿using UnityEngine;

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
            _atraObj = Instantiate(_atraPrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        }
        _playerStatuses.isAtraGunHanded = false;
        _playerStatuses.isWeaponHanded = true;
        _playerStatuses.attackInvoked = false;
    }

    public void AddAtraForce(Transform playerTransform, Rigidbody playerRb)
    {
        //Debug.Log("Atra");
        if (_atraObj)
        {
            _atraObj.TryGetComponent(out _atra);
            _atra.AddAtraForce(playerTransform, playerRb);
        }
        else
        {
            _playerStatuses.isAtraForceEnabled = false;
        }
    }
}
