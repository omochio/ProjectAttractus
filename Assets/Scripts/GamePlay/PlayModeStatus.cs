using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Status/PlayModeStatus")]
public class PlayModeStatus : ScriptableObject
{
    void OnEnable()
    {
        IsReady = _isReady;
        IsPlaying = _isPlaying;
        IsPaused = _isPaused;
        IsGameOver = _isGameOver;
    }

    [NonSerialized]
    bool _isReady = true;
    public bool IsReady
    {
        get => _isReady;
        set => _isReady = value;
    }

    [NonSerialized]
    bool _isPlaying = false;
    public bool IsPlaying
    {
        get => _isPlaying;
        set => _isPlaying = value;
    }

    [NonSerialized]
    bool _isPaused = false;
    public bool IsPaused
    {
        get => _isPaused;
        set => _isPaused = value;
    }

    [NonSerialized]
    bool _isGameOver = false;
    public bool IsGameOver
    {
        get => _isGameOver;
        set => _isGameOver = value;
    }

}