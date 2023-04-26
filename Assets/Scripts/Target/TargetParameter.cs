using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TargetParameter")]
public class TargetParameter : ScriptableObject
{
    [SerializeField]
    int _score;
    public int Score
    { get => _score; }

    [SerializeField]
    float _initHelth;
    public float InitHelth
    { get => _initHelth; }

    [SerializeField]
    float _moveSpeed;
    public float MoveSpeed
    { get => _moveSpeed; }

    [SerializeField]
    float _minRotateInterval;
    public float MinRotateInterval
    { get => _minRotateInterval; }

    [SerializeField]
    float _maxRotateInterval;
    public float MaxRotateInterval
    { get => _maxRotateInterval; }
}