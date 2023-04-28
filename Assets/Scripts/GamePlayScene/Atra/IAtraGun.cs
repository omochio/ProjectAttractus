using UnityEngine;

public interface IAtraGun
{
    public void Shot();
    public void AddAtraForce(Transform playerTransform, Rigidbody playerRb);
}
