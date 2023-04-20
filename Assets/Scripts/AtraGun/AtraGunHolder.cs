using System.Collections.Generic;
using UnityEngine;

public class AtraGunHolder : MonoBehaviour
{
    [SerializeField]
    List<AtraGun> _atraGuns = new();

    int _currentAtraGunIndex = 0;

    void OnEnable()
    {
        foreach (AtraGun atraGun in _atraGuns)
        {
            atraGun.gameObject.SetActive(false);
        }
        _atraGuns[_currentAtraGunIndex].gameObject.SetActive(true);
    }

    public IAtraGun GetCurrentAtraGun()
    {
        return _atraGuns[_currentAtraGunIndex];
    }

    public void SelectWeapon(int atraGunIdx)
    {
        if (atraGunIdx >= 0 && atraGunIdx < _atraGuns.Count)
        {
            _currentAtraGunIndex = atraGunIdx;
        }
    }

    public void AddAtraGunIdx(int val)
    {
        _currentAtraGunIndex += val;
        if (_currentAtraGunIndex < 0)
        {
            _currentAtraGunIndex = _atraGuns.Count - 1 - Mathf.Abs(_currentAtraGunIndex) % _atraGuns.Count;
        }
        else if (_currentAtraGunIndex > _atraGuns.Count - 1)
        {
            _currentAtraGunIndex = (_currentAtraGunIndex - _atraGuns.Count - 1) % _atraGuns.Count;
        }
    }
}