using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    readonly List<Weapon> _weapons = new();
    public List<Weapon> weapons
    { 
        get => _weapons; 
    }

    int _currentWeaponIndex = 0;
    public int currentWeaponIndex
    {
        get => _currentWeaponIndex;
        set
        {
            if (0 <= value && value < _weapons.Count)
                _currentWeaponIndex = value;
        }
    }
    
    void Start()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        _weapons[_currentWeaponIndex].gameObject.SetActive(true);
    }

    public Weapon GetCurrentWeapon()
    {
        return _weapons[_currentWeaponIndex];
    }

}
