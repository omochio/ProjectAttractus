using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    [SerializeField]
    WeaponHolder _weaponHolder;
    [SerializeField]
    PlayerStatus _playerStatus;
    TMP_Text _ammoText;

    void Awake()
    {
        TryGetComponent(out _ammoText);
    }

    // Update is called once per frame
    void Update()
    {
        var weapon = _weaponHolder.GetCurrentWeapon();
        if (_playerStatus.ReloadInvoked)
        {
            _ammoText.text = $"Ammo: <color=#FFFF00FF>Reloading</color>";
        }
        else
        {
            _ammoText.text = $"Ammo: {weapon.AmmoCount}";
        }
    }
}
