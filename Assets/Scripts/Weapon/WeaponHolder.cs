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
}
