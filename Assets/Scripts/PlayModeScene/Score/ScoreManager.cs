using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int _score = 0;
    public int Score
    {
        get => _score;
    }

    public void AddScore(int addVal)
    {
        _score += addVal;
    }
}