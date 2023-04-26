using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int _score;
    public int Score
    { get => _score; }
    
    public void AddScore(int addVal)
    {
        _score += addVal;
        Debug.Log(_score);
    }
}