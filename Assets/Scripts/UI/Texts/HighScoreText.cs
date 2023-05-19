using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{
    [SerializeField]
    ScoreManager _scoreManager;
    TMP_Text _scoreText;

    void Awake()
    {
        TryGetComponent(out _scoreText);
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = $"HighScore: {_scoreManager.HighScore}";
    }
}