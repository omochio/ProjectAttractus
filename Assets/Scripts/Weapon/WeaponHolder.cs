using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    List<Weapon> _weapons = new();

    int _currentWeaponIndex = 0;


    void OnEnable()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        _weapons[_currentWeaponIndex].gameObject.SetActive(true);
    }

    public IWeapon GetCurrentWeapon()
    {
        return _weapons[_currentWeaponIndex];
    }

    public void SelectWeapon(int weaponIdx)
    {
        if (weaponIdx >= 0 && weaponIdx < _weapons.Count)
        {
            _currentWeaponIndex = weaponIdx;
        }
    }

    // TODO: Test needed
    public void AddWeaponIdx(int val)
    {
        _currentWeaponIndex += val;
        if (_currentWeaponIndex < 0)
        {
            _currentWeaponIndex = _weapons.Count - 1 - Mathf.Abs(_currentWeaponIndex) % _weapons.Count;
        }
        else if (_currentWeaponIndex > _weapons.Count - 1)
        {
            _currentWeaponIndex = (_currentWeaponIndex - _weapons.Count - 1) % _weapons.Count;
        }
    }
}
