using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Parameter/PlayModeParameter")]
public class PlayModeParameter : ScriptableObject
{
    [SerializeField, Tooltip("Second")]
    float _readyTime;
    public float ReadyTime
    {
        get => _readyTime;
    }

    [SerializeField, Tooltip("Second")]
    float _timeLimit;
    public float TimeLimit
    {
        get => _timeLimit;
    }
}