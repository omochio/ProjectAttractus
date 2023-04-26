using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AtraParameter")]
public class AtraParameter : ScriptableObject
{
    [SerializeField]
    float _speed;
    public float Speed
    { get => _speed; }

    [SerializeField]
    float _mass;
    public float Mass
    { get => _mass; }

    [SerializeField]
    float _lifeTime;
    public float LifeTime
    { get => _lifeTime; }

    [SerializeField]
    float _forceValidDistance;
    public float ForceValidDistance
    { get => _forceValidDistance; }
}