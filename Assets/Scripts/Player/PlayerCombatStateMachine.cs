using IceMilkTea.Core;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombatStateMachine : MonoBehaviour
{
    class PlayerCombatStateBase : ImtStateMachine<PlayerCombatStateMachine, StateEvent>.State
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
        Attack,
        Reload
    }

    ImtStateMachine<PlayerCombatStateMachine, StateEvent> _stateMachine;

    UnityAction _switchState;

    WeaponHolder _weaponHolder;
    PlayerStatuses _playerStatuses;

    void Awake()
    {
        TryGetComponent(out _weaponHolder);
        TryGetComponent(out _playerStatuses);

        _stateMachine = new ImtStateMachine<PlayerCombatStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<PendingState>();

        // Any states
        _stateMachine.AddAnyTransition<PendingState>(StateEvent.Pending);

        // From DoNothing
        _stateMachine.AddTransition<PendingState, AttackState>(StateEvent.Attack);
        _stateMachine.AddTransition<PendingState, ReloadState>(StateEvent.Reload);

        // From Attack
        _stateMachine.AddTransition<AttackState, ReloadState>(StateEvent.Reload);

        // From Reload
        _stateMachine.AddTransition<ReloadState, AttackState>(StateEvent.Attack);
    }

    public void UpdateState()
    {
        _stateMachine.Update();
        //Debug.Log(_stateMachine.CurrentStateName);
    }

    public void SwitchState()
    {
        _switchState.Invoke();
    }

    class PendingState : PlayerCombatStateBase 
    {
        protected override void SwitchState()
        {
            if (Context._playerStatuses.attackInvoked)
            {
                StateMachine.SendEvent(StateEvent.Attack);
            }
            else if (Context._playerStatuses.reloadInvoked)
            {
                StateMachine.SendEvent(StateEvent.Reload);
            }
        }
    }
    
    class AttackState : PlayerCombatStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._weaponHolder.GetCurrentWeapon().InitElapsedTime();
        }

        protected internal override void Update()
        {
            Context._weaponHolder.GetCurrentWeapon().Attack();
        }

        protected override void SwitchState()
        {
            if (Context._playerStatuses.attackInvoked)
            {
                if (Context._playerStatuses.reloadInvoked)
                {
                    StateMachine.SendEvent(StateEvent.Reload);
                }
            }
            else
            {
                StateMachine.SendEvent(StateEvent.Pending);
            }
        }
    }
    
    class ReloadState : PlayerCombatStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._weaponHolder.GetCurrentWeapon().InitElapsedTime();
        }

        protected internal override void Update()
        {
            Context._weaponHolder.GetCurrentWeapon().Reload();
        }

        protected override void SwitchState()
        {
            if (Context._playerStatuses.reloadInvoked)
            {
                if (Context._playerStatuses.attackInvoked)
                {
                    stateMachine.SendEvent(StateEvent.Attack);
                }
            }
            else
            {
                StateMachine.SendEvent(StateEvent.Pending);
            }
        }
    }
}
