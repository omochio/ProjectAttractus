using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField]
    SceneParameter _sceneParam;

    void Awake()
    {
        Cursor.visible = _sceneParam.IsCursorVisible;
        Cursor.lockState = _sceneParam.CursorMode;
    }
}