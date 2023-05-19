using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [System.Serializable]
    public class SaveData
    {
        public int highScore = 0;
    }

    SaveData _saveData = new();

    readonly string SAVE_DATA_FILE_PATH = $"{System.Environment.GetEnvironmentVariable("USERPROFILE")}/AppData/Local/Atra/save.json";
    readonly string SAVE_DATA_DIRECTORY_PATH = $"{System.Environment.GetEnvironmentVariable("USERPROFILE")}/AppData/Local/Atra";

    int _score = 0;
    public int Score
    {
        get => _score;
    }

    public int HighScore
    {
        get => _saveData.highScore;
    }

    bool isNewRecord = false;
    public bool IsNewRecord
    {
        get 
        { 
            if (_saveData.highScore < _score)
            {
                isNewRecord = true;
            }
            else
            {
                isNewRecord = false;
            }
            return isNewRecord;
        }
    }

    void Awake()
    {
        if (!Directory.Exists(SAVE_DATA_DIRECTORY_PATH)) 
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY_PATH);
        }
        else if (File.Exists(SAVE_DATA_FILE_PATH))
        {
            _saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(SAVE_DATA_FILE_PATH));
        }
    }

    public void AddScore(int addVal)
    {
        _score += addVal;
    }

    public void SaveHightScore()
    {
        if (IsNewRecord)
        {
            _saveData.highScore = _score;
            File.WriteAllText(SAVE_DATA_FILE_PATH, JsonUtility.ToJson(_saveData));
        }
    }
}