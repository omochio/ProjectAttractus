using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Parameter/WeaponParameter")]
public class WeaponParameter : ScriptableObject
{
    [SerializeField]
    float _damage;
    public float Damage
    { get => _damage; }

    [SerializeField, Tooltip("SPM")]
    float _fireRate;
    public float FireRate 
    { get => _fireRate;}

    [SerializeField]
    int _magazineSize;
    public int MagazineSize
    { get => _magazineSize; }

    [SerializeField]
    bool _isAutomatic;
    public bool IsAutomatic 
    { get => _isAutomatic; }

    [SerializeField]
    float _reloadTime;
    public float ReloadTime
    { get => _reloadTime; }

    [SerializeField]
    GameObject _projectileObj;
    public GameObject ProjectileObj
    { get  => _projectileObj; }
}