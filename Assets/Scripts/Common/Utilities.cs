using UnityEngine;

public static class Utilities
{
    /// <summary>
    /// Frame-rate independent Lerp
    /// </summary>
    /// <param name="value">Current value</param>
    /// <param name="target">Target value</param>
    /// <param name="r">Lerp speed</param>
    /// <param name="dt">Delta time</param>
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

    /// <summary>
    /// Extended Random.Range() for Vector2 and Vector3
    /// This returns Vector object assigned random value to element by element
    /// </summary>
    public static Vector2 VecRandRange(Vector2 minInclusive, Vector2 maxInclusive)
    {
        return new Vector2(
            Random.Range(minInclusive.x, maxInclusive.x),
            Random.Range(minInclusive.y, maxInclusive.y));
    }
    public static Vector3 VecRandRange(Vector3 minInclusive, Vector3 maxInclusive)
    {
        return new Vector3(
            Random.Range(minInclusive.x, maxInclusive.x), 
            Random.Range(minInclusive.y, maxInclusive.y),
            Random.Range(minInclusive.z, maxInclusive.z));
    }
}
