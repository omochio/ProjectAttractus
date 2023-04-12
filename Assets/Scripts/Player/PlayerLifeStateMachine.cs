using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;

public class PlayerLifeStateMachine : MonoBehaviour
{
    class PlayerLifeStateBase : ImtStateMachine<PlayerLifeStateMachine, StateEvent>.State
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

    ImtStateMachine<PlayerLifeStateMachine, StateEvent> _stateMachine;

    UnityAction _switchState;

    PlayerStatuses _playerStatuses;
    PlayerMovementManager _playerMovementManager;
    PlayerMovementStateMachine _playerMovementStateMachine;
    PlayerCombatStateMachine _playerCombatStateMachine;

    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        TryGetComponent(out _playerMovementManager);
        TryGetComponent(out _playerMovementStateMachine);
        TryGetComponent(out _playerCombatStateMachine);


        _stateMachine = new ImtStateMachine<PlayerLifeStateMachine, StateEvent>(this);

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
        //Debug.Log(_stateMachine.CurrentStateName);
    }

    class PendingState : PlayerLifeStateBase { }

    class AliveState : PlayerLifeStateBase
    {
        protected internal override void Update()
        {
            Context._playerMovementStateMachine.UpdateState();
            Context._playerCombatStateMachine.UpdateState();

            Context._playerMovementManager.ApplyVelocityChange();

            Context._playerMovementStateMachine.SwitchState();   
            Context._playerCombatStateMachine.SwitchState();
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.isAlive)
            {
                StateMachine.SendEvent(StateEvent.Die);
            }
        }
    }
    
    class DieState : PlayerLifeStateBase { }

}
