using System.Collections.Generic;
using UnityEngine;

public class AtraShooterHolder : MonoBehaviour
{
    [SerializeField]
    List<AtraShooter> _atraShooters = new();

    int _currentAtraIndex = 0;

    void OnEnable()
    {
        foreach (AtraShooter atraShooter in _atraShooters)
        {
            atraShooter.gameObject.SetActive(false);
        }
        _atraShooters[_currentAtraIndex].gameObject.SetActive(true);
    }

    public IAtraShooter GetCurrentAtraShooter()
    {
        return _atraShooters[_currentAtraIndex];
    }
}