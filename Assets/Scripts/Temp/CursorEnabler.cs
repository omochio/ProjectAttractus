using UnityEngine;

public class CursorEnabler : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
