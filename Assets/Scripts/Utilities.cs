using UnityEngine;

public static class Utilities
{
    public static float FRILerp(float value, float target, float r, float dt)
    {
        return Mathf.Lerp(value, target, 1 - Mathf.Exp(-r * dt));
    }
    public static Vector2 FRILerp(Vector2 value, Vector2 target, float r, float dt)
    {
        return new Vector2(
            FRILerp(value.x, target.x, r, dt),
            FRILerp(value.y, target.y, r, dt));
    }
    public static Vector3 FRILerp(Vector3 value, Vector3 target, float r, float dt)
    {
        return new Vector3(
            FRILerp(value.x, target.x, r, dt),
            FRILerp(value.y, target.y, r, dt),
            FRILerp(value.z, target.z, r, dt));
    }

}
