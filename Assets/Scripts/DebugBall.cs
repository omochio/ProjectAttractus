using UnityEngine;

public class DebugBall : MonoBehaviour
{
    float _elapsedTime;

    // Update is called once per frame
    void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 1f)
        {
            Destroy(gameObject);
        }

        transform.position += transform.rotation * (1000f * Time.deltaTime * Vector3.forward);
    }
}
