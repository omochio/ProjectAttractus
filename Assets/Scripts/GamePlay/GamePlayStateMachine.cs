using UnityEngine;
using IceMilkTea.Core;

public class GamePlayStateMachine : MonoBehaviour
{
    class GamePlayStateBase : ImtStateMachine<GamePlayStateMachine, StateEvent>.State { }

    enum StateEvent
    {
        Ready,
        Playing,
        Pause,
        GameOver
    }

    ImtStateMachine<GamePlayStateMachine, StateEvent> _stateMachine;


    void Awake()
    {
        _stateMachine = new ImtStateMachine<GamePlayStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<ReadyState>();
    }

    void Update()
    {
        _stateMachine.Update();    
    }

    class ReadyState : GamePlayStateBase
    {

    }
}
