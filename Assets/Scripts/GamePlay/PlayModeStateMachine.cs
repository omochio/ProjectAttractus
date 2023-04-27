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
        GameOver
    }

    ImtStateMachine<PlayModeStateMachine, StateEvent> _stateMachine;

    [SerializeField]
    PlayModeParameter _playModeParameter;
    [SerializeField]
    PlayModeStatus _playModeStatus;
    [SerializeField]
    TMP_Text _readyText;
    [SerializeField]
    TMP_Text _timeLimitText;


    UnityAction _switchState;

    void Awake()
    {
        _stateMachine = new ImtStateMachine<PlayModeStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<ReadyState>();

        // From ReadyState
        _stateMachine.AddTransition<ReadyState, PlayingState>(StateEvent.Playing);
        _stateMachine.AddTransition<ReadyState, PauseState>(StateEvent.Pause);

        // From PlayingState
        _stateMachine.AddTransition<PlayingState, GameOverState>(StateEvent.GameOver);
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
        Timer _timer;

        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
            _timer = new(Time.unscaledTime, Context._playModeParameter.ReadyTime);
        }

        protected internal override void Update()
        {
            Context._readyText.text = $"{(int)_timer.GetRemainTime(Time.unscaledTime)}";
            if (_timer.IsTimeUp(Time.unscaledTime))
            {
                Context._playModeStatus.IsPlaying = true;
            }
        }

        protected internal override void Exit()
        {
            Context._readyText.text = "<color=#FF0000FF>START!!</color>";
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
        Timer _readyTextTimer;
        Timer _gameTimer;

        protected internal override void Enter()
        {
            base.Enter();
            _readyTextTimer = new(Time.time, Context._playModeParameter.ReadyTextTime);
            _gameTimer = new(Time.time, Context._playModeParameter.TimeLimit);
        }

        protected internal override void Update()
        {
            Context._timeLimitText.text = $"Time: {(int)_gameTimer.GetRemainTime(Time.time)}";
            if (_readyTextTimer.IsTimeUp(Time.time))
            {
                Context._readyText.gameObject.SetActive(false);
            }
            if (_gameTimer.IsTimeUp(Time.time))
            {
                Context._playModeStatus.IsGameOver = true;
            }
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
                stateMachine.SendEvent(StateEvent.GameOver);
            }
        }
    }

    class GameOverState : PlayModeStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
        }

        protected internal override void Exit()
        {
            Time.timeScale = 1f;
        }
    }

    class PauseState : PlayModeStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Time.timeScale = 0f;
        }

        protected internal override void Exit()
        {
            Time.timeScale = 1f;
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
