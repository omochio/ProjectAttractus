using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    List<Weapon> _weapons = new();

    [SerializeField]
    PlayerStatus _playerStatus;

    [SerializeField]
    TMP_Text _ammoText;

    int _currentWeaponIndex = 0;


    void OnEnable()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.Init(_playerStatus, _ammoText);
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
