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

    [SerializeField]
    GameObject _weaponHolderObj;
    WeaponHolder _weaponHolder;

    [SerializeField]
    GameObject _atraGunHolderObj;
    AtraGunHolder _atraGunHolder;

    [SerializeField]
    PlayerStatus _playerStatus;

    void Awake()
    {
        _weaponHolderObj.TryGetComponent(out _weaponHolder);
        _atraGunHolderObj.TryGetComponent(out _atraGunHolder);

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
    }

    public void SwitchState()
    {
        _switchState.Invoke();
    }

    class PendingState : PlayerCombatStateBase 
    {
        protected override void SwitchState()
        {
            if (Context._playerStatus.AttackInvoked)
            {
                StateMachine.SendEvent(StateEvent.Attack);
            }
            else if (Context._playerStatus.ReloadInvoked)
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
            if (Context._playerStatus.IsWeaponHanded)
            {
                Context._weaponHolder.GetCurrentWeapon().ResetTimeCount();
            }
        }

        protected internal override void Update()
        {
            if (Context._playerStatus.IsWeaponHanded)
            {
                Context._weaponHolder.GetCurrentWeapon().Shot();
            }
            else if (Context._playerStatus.IsAtraGunHanded)
            {
                Context._atraGunHolder.GetCurrentAtraGun().Shot();
            }
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.AttackInvoked)
            {
                if (Context._playerStatus.IsWeaponHanded)
                {
                    if (Context._playerStatus.ReloadInvoked)
                    {
                        StateMachine.SendEvent(StateEvent.Reload);
                    }
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
            Context._weaponHolder.GetCurrentWeapon().ResetTimeCount();
        }

        protected internal override void Update()
        {
            Context._weaponHolder.GetCurrentWeapon().Reload();
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatus.ReloadInvoked)
            {
                if (Context._playerStatus.AttackInvoked)
                {
                    StateMachine.SendEvent(StateEvent.Attack);
                }
                else
                {
                    StateMachine.SendEvent(StateEvent.Pending);
                }
            }
        }
    }
}
