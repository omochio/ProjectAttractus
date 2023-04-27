public class Timer
{
    readonly float _startTime;
    readonly float _setTime;

    public Timer(float startTime, float setTime)
    {
        _startTime = startTime;
        _setTime = setTime;
    }

    public bool IsTimeUp(float currentTime)
    {
        if (currentTime - _startTime >= _setTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}