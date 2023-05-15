using UnityEngine;

public class PlayerOrientationManager : MonoBehaviour
{
    [SerializeField]
    Transform _playerTransform;

    void Update()
    {
        _playerTransform.rotation = Quaternion.Euler(Vector3.up * Camera.main.transform.rotation.eulerAngles.y);
    }
}
