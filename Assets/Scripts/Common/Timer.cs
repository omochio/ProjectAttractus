public class Timer
{
    float _startTime;
    float _setTime;
    bool _isPaused = false;

    public Timer(float startTime, float setTime)
    {
        _startTime = startTime;
        _setTime = setTime;
    }

    public bool IsTimeUp(float currentTime)
    {
        if (_isPaused)
        {
            return false;
        }

        if (currentTime - _startTime >= _setTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetRemainTime(float currentTime)
    {
        if (_isPaused)
        {
            return _setTime;
        }

        if (IsTimeUp(currentTime))
        {
            return 0;
        }
        else
        {
            return _setTime - (currentTime - _startTime);
        }
    }

    public void PauseOrResume(float currentTime)
    {
        if (IsTimeUp(currentTime))
        {
            return;
        }

        if (_isPaused)
        {
            _startTime = currentTime;
            _isPaused = false;
        }
        else
        {
            _setTime = GetRemainTime(currentTime);
            _isPaused = true;
        }    
    }
}