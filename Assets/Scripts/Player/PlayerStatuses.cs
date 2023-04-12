using UnityEngine;

public class PlayerStatuses : MonoBehaviour
{
    public bool moveInvoked = false;
    public bool sprintInvoked = false;
    public bool jumpInvoked = false;
    public bool crouchOrSlideInvoked = false;
    public bool attackInvoked = false;
    public bool reloadInvoked = false;

    public bool isAlive = true;

    public bool isGrounded = true;
    public bool isSlidable = false;
    public bool isSlideCooling = false;
    //public bool isGravityEnabled = true;

    float _slideElapsedTime;
    public float slideElapsedTime
    {
        get { return _slideElapsedTime; }
        set { _slideElapsedTime = value; }
    }

}