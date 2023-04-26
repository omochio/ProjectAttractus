using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text _scoreText;

    int _score;
    public int Score
    { get => _score; }

    public void AddScore(int addVal)
    {
        _score += addVal;
    }

    void Update()
    {
        _scoreText.text = $"Score: {_score}";
    }
}