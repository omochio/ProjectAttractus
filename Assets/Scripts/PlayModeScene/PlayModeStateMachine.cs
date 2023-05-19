using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;
using TMPro;

public class PlayModeStateMachine : MonoBehaviour
{
    class PlayModeStateBase : ImtStateMachine<PlayModeStateMachine, StateEvent>.State
    {
        protected internal override void Enter()
        {
            Context._switchState = SwitchState;
        }

        protected virtual void SwitchState() { } 
    }

    enum StateEvent
    {
        Ready,
        Playing,
        Pause,
        GameSet
    }

    ImtStateMachine<PlayModeStateMachine, StateEvent> _stateMachine;

    [SerializeField]
    PlayModeParameter _playModeParameter;
    [SerializeField]
    PlayModeStatus _playModeStatus;
    [SerializeField]
    GameObject _playingCanvas;
    [SerializeField]
    TMP_Text _readyText;
    [SerializeField]
    TMP_Text _timeLimitText;
    [SerializeField]
    GameObject _pausedCanvas;
    [SerializeField]
    GameObject _gameOverCanvas;
    [SerializeField]
    GameObject _newRecordTextObj;
    [SerializeField]
    ScoreManager _scoreManager;

    UnityAction _switchState;

    void Awake()
    {
        _stateMachine = new ImtStateMachine<PlayModeStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<ReadyState>();

        // From ReadyState
        _stateMachine.AddTransition<ReadyState, PlayingState>(StateEvent.Playing);
        _stateMachine.AddTransition<ReadyState, PauseState>(StateEvent.Pause);

        // From PlayingState
        _stateMachine.AddTransition<PlayingState, GameSetState>(StateEvent.GameSet);
        _stateMachine.AddTransition<PlayingState, PauseState>(StateEvent.Pause);

        // From PauseState
        _stateMachine.AddTransition<PauseState, ReadyState>(StateEvent.Ready);
        _stateMachine.AddTransition<PauseState, PlayingState>(StateEvent.Playing);
    }

    void Update()
    {
        _stateMachine.Update();
        _switchState.Invoke();
    }

    class ReadyState : PlayModeStateBase
    {
        Timer _timer = null;

        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            if (_timer != null)
            {
                _timer.PauseOrResume(Time.unscaledTime);
            }
            else
            {
                _timer = new(Time.unscaledTime, Context._playModeParameter.ReadyTime);
            }
        }

        protected internal override void Update()
        {
            Context._readyText.text = $"{(int)_timer.GetRemainTime(Time.unscaledTime)}";
            if (_timer.GetRemainTime(Time.unscaledTime) < 1f)
            {
                Context._readyText.text = "<color=#FF0000FF>START!!</color>";
                if (_timer.IsTimeUp(Time.unscaledTime))
                {
                    Context._playModeStatus.IsPlaying = true;
                }
            }
        }

        protected internal override void Exit()
        {
            _timer.PauseOrResume(Time.unscaledTime);
            Time.timeScale = 1f;
        }

        protected override void SwitchState()
        {
            if (Context._playModeStatus.IsPaused)
            {
                stateMachine.SendEvent(StateEvent.Pause);
            }
            else if (Context._playModeStatus.IsPlaying)
            {
                Context._playModeStatus.IsReady = false;
                stateMachine.SendEvent(StateEvent.Playing);
            }
        }
    }

    class PlayingState : PlayModeStateBase
    {
        Timer _timer = null;

        protected internal override void Enter()
        {
            base.Enter();
            if (_timer != null)
            {
                _timer.PauseOrResume(Time.time);
            }
            else
            {
                _timer = new(Time.time, Context._playModeParameter.TimeLimit);
            }
            Context._readyText.gameObject.SetActive(false);
        }

        protected internal override void Update()
        {
            Context._timeLimitText.text = $"Time: {(int)_timer.GetRemainTime(Time.time)}";
            if (_timer.IsTimeUp(Time.time))
            {
                Context._playModeStatus.IsGameOver = true;
            }
        }

        protected internal override void Exit()
        {
            _timer.PauseOrResume(Time.time);
        }

        protected override void SwitchState()
        {
            if (Context._playModeStatus.IsPaused)
            {
                stateMachine.SendEvent(StateEvent.Pause);
            }
            else if (Context._playModeStatus.IsGameOver)
            {
                Context._playModeStatus.IsPlaying = false;
                stateMachine.SendEvent(StateEvent.GameSet);
            }
        }
    }

    class GameSetState : PlayModeStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Context._playingCanvas.SetActive(false);
            Context._gameOverCanvas.SetActive(true);
            if (Context._scoreManager.IsNewRecord)
            {
                Context._newRecordTextObj.SetActive(true);
            }
            // temp
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Context._scoreManager.SaveHightScore();
        }

        protected internal override void Exit()
        {
            Time.timeScale = 1f;
            Context._gameOverCanvas.SetActive(false);
        }
    }

    class PauseState : PlayModeStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            Context._playingCanvas.SetActive(false);
            Context._pausedCanvas.SetActive(true);
            // temp
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        protected internal override void Exit()
        {
            Time.timeScale = 1f;
            Context._playingCanvas.SetActive(true);
            Context._pausedCanvas.SetActive(false);
            // temp
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        protected override void SwitchState()
        {
            if (!Context._playModeStatus.IsPaused)
            {
                if(Context._playModeStatus.IsReady)
                {
                    stateMachine.SendEvent(StateEvent.Ready);
                }
                else if (Context._playModeStatus.IsPlaying)
                {
                    stateMachine.SendEvent(StateEvent.Playing);
                }
            }
        }
    }
}
