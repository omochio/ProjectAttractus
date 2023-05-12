using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text _scoreText;

    [SerializeField]
    PlayModeStatus _playModeStatus;

    //int _score;
    //public int Score
    //{ get => _score; }

    public void AddScore(int addVal)
    {
        _playModeStatus.Score += addVal;
    }

    void Update()
    {
        _scoreText.text = $"Score: {_playModeStatus.Score}";
    }
}