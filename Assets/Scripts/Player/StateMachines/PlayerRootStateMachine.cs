using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;

public class PlayerRootStateMachine : MonoBehaviour
{
    class PlayerRootStateBase : ImtStateMachine<PlayerRootStateMachine, StateEvent>.State
    {
        protected internal override void Enter()
        {
            Context._switchState = SwitchState;
        }
        protected virtual void SwitchState() { }
    }

    enum StateEvent
    {
        Pending,
        Alive,
        Die
    }

    ImtStateMachine<PlayerRootStateMachine, StateEvent> _stateMachine;

    UnityAction _switchState;

    PlayerStatus _playerStatus;
    PlayerMovementStateMachine _playerMovementStateMachine;
    PlayerCombatStateMachine _playerCombatStateMachine;

    void Awake()
    {
        TryGetComponent(out _playerStatus);
        TryGetComponent(out _playerMovementStateMachine);
        TryGetComponent(out _playerCombatStateMachine);


        _stateMachine = new ImtStateMachine<PlayerRootStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<AliveState>();

        // Any states
        _stateMachine.AddAnyTransition<PendingState>(StateEvent.Pending);

        // From Pending
        _stateMachine.AddTransition<PendingState, AliveState>(StateEvent.Alive);
        _stateMachine.AddTransition<PendingState, DieState>(StateEvent.Die);

        // From Live
        _stateMachine.AddTransition<AliveState, DieState>(StateEvent.Die);
        // From Die
        _stateMachine.AddTransition<DieState, AliveState>(StateEvent.Alive);
    }

    void FixedUpdate()
    {
        _stateMachine.Update();
        _switchState.Invoke();
    }

    class PendingState : PlayerRootStateBase { }

    class AliveState : PlayerRootStateBase
    {
        protected internal override void Update()
        {
            Context._playerMovementStateMachine.UpdateState();
            Context._playerCombatStateMachine.UpdateState();

            Context._playerMovementStateMachine.SwitchState();   
            Context._playerCombatStateMachine.SwitchState();
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatus.isAlive)
            {
                StateMachine.SendEvent(StateEvent.Die);
            }
        }
    }
    
    class DieState : PlayerRootStateBase { }

}
