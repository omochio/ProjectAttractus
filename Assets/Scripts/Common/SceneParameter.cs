using UnityEngine;

[CreateAssetMenu(menuName="ScriptableObject/Parameter/SceneParameter")]
public class SceneParameter : ScriptableObject
{
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