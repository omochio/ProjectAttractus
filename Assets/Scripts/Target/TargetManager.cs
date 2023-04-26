using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField]
    int _maxGeneralTargetCount;

    [SerializeField]
    GameObject _generalTargetObj;

    [SerializeField]
    Vector3 _minTargetPos;

    [SerializeField]
    Vector3 _maxTargetPos;

    [SerializeField]
    ScoreManager _scoreManager;

    readonly List<GameObject> _generalTargetList = new();

    void OnEnable()
    {
        CreateTargets();    
    }

    void Update()
    {
        foreach(GameObject obj in _generalTargetList)
        {
            obj.TryGetComponent(out Target t);
            if (t.IsBroken == true)
            {
                obj.transform.position = Utilities.VecRandRange(_minTargetPos, _maxTargetPos);
                t.IsBroken = false;
            }
        }
    }

    public void CreateTargets()
    {
        for (var i = 0; i < _maxGeneralTargetCount; ++i) 
        {
            var obj = Instantiate(_generalTargetObj, gameObject.transform);
            obj.transform.position = Utilities.VecRandRange(_minTargetPos, _maxTargetPos);
            obj.TryGetComponent(out Target t);
            t.Init(_scoreManager);
            _generalTargetList.Add(obj);
        }
    }

}