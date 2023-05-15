using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    Button _button;

    void Awake()
    {
        TryGetComponent(out _button);
        _button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick() { }
}