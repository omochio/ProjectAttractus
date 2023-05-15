using UnityEngine;

public class SceneChangeButton : ButtonBase
{
    [SerializeField]
    string _nextSceneName;

    protected override void OnClick()
    {
        SceneChanger.SceneChange(_nextSceneName);
    }
}
