using TMPro;
using UnityEngine;

public class LodingTextController : MonoBehaviour
{
    [SerializeField]
    TMP_Text _loadingText;

    [SerializeField]
    float _textChangeDuration;

    readonly string _text = "Loading";
    int _changeCount = 0;

    Timer _timer;

    void Start()
    {
        _timer = new(Time.time, _textChangeDuration);
    }

    void Update()
    {
        if (_timer.IsTimeUp(Time.time))
        {
            _timer = new(Time.time, _textChangeDuration);
            if (_changeCount < 3)
            {
                _loadingText.text = _text + new string('.' , _changeCount + 1);
                ++_changeCount;
            }
            else
            {
                _loadingText.text = _text;
                _changeCount = 0;
            }
        }
    }
}
