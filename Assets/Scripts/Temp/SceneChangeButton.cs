using UnityEngine;
using UnityEngine.UI;

class SceneChangeButton : MonoBehaviour
{
    [SerializeField]
    string _nextSceneName;

    Button _button;

    void Awake()
    {
        TryGetComponent(out _button);
        _button.onClick.AddListener(() =>
        {
            SceneChanger.SceneChange(_nextSceneName);
        });
    }
}
