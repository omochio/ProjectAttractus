using System.Data.Common;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    float _damage;
    [SerializeField]
    float _fireRate;
    [SerializeField]
    int _magazineSize;
    [SerializeField]
    bool _isAutomatic;
    [SerializeField]
    float _reloadTime;

    float _elapsedTime;
    float _bulletsCount;

    // TODO: Reconsider initialize purpose
    [SerializeField]
    PlayerStatuses _playerStatuses;
    

    void Awake()
    {
        _bulletsCount = _magazineSize;
    }

    public void InitElapsedTime()
    {
        _elapsedTime = 0f;
    }

    public void Attack()
    {
        _elapsedTime += Time.fixedDeltaTime;

        if (_bulletsCount == 0)
        {
            _playerStatuses.reloadInvoked = true;
            _playerStatuses.attackInvoked = false;
        }
        else
        {
            Ray ray = new(Camera.main.transform.position, Camera.main.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            if (_isAutomatic)
            {
                if (_elapsedTime % (1f / _fireRate) == 0)
                {
                    --_bulletsCount;
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
            else
            {
                --_bulletsCount;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                _playerStatuses.attackInvoked = false;
            }
        }
    }

    public void Reload() 
    {
        _elapsedTime += Time.fixedDeltaTime;

        if (_elapsedTime >= _reloadTime)
        {
            _bulletsCount = _magazineSize;
            _playerStatuses.reloadInvoked = false;
        }
    }
}
