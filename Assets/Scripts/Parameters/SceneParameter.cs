using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Parameter/SceneParameter")]
public class SceneParameter : ScriptableObject
{
    [SerializeField]
    List<ScriptableObject> _statuses;
    public List<ScriptableObject> Statuses
    {
        get => _statuses;
    }

    [SerializeField]
    bool _isCursorVisible;
    public bool IsCursorVisible
    {
        get => _isCursorVisible;
    }

    [SerializeField]
    CursorLockMode _cursorMode;
    public CursorLockMode CursorMode
    {
        get => _cursorMode;
    }
}