using UnityEngine;

public class PlayModeStatus : MonoBehaviour
{
    bool _isReady = true;
    public bool IsReady
    {
        get => _isReady;
        set => _isReady = value;
    }
   
    bool _isPlaying = false;
    public bool IsPlaying
    {
        get => _isPlaying;
        set => _isPlaying = value;
    }

    bool _isPaused = false;
    public bool IsPaused
    {
        get => _isPaused;
        set => _isPaused = value;
    }

    bool _isGameOver = false;
    public bool IsGameOver
    {
        get => _isGameOver;
        set => _isGameOver = value;
    }

}