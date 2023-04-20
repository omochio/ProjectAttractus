using System.Collections.Generic;
using UnityEngine;

public class ShootableHolder : MonoBehaviour
{
    [SerializeField]
    List<Weapon> _weapons = new();

    [SerializeField]
    List<Atra> _atras = new();

    List<IShootabe> _shootables = new();
    public List<IShootabe> Shootabes
    { get { return _shootables; } }

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

    void Awake()
    {
        (_shootables = _weapons.ConvertAll(x => x as IShootabe)).AddRange(_atras.ConvertAll(x => x as IShootabe));
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
