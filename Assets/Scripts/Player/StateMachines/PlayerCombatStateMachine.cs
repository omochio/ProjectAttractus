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

    PlayerStatuses _playerStatuses;

    void Awake()
    {
        _weaponHolderObj.TryGetComponent(out _weaponHolder);
        _atraGunHolderObj.TryGetComponent(out _atraGunHolder);
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
        //if (_playerStatuses.isWeaponHanded)
        //{
        //    _weaponHolderObj.SetActive(true);
        //    _atraGunHolderObj.SetActive(false);
        //}
        //else if (_playerStatuses.isAtraGunHanded)
        //{
        //    _weaponHolderObj.SetActive(false);
        //    _atraGunHolderObj.SetActive(true);
        //}

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
            if (Context._playerStatuses.isWeaponHanded)
            {
                Context._weaponHolder.GetCurrentWeapon().InitTimeCount();
            }
        }

        protected internal override void Update()
        {
            if (Context._playerStatuses.isWeaponHanded)
            {
                Context._weaponHolder.GetCurrentWeapon().Shot();
            }
            else if (Context._playerStatuses.isAtraGunHanded)
            {
                Context._atraGunHolder.GetCurrentAtraGun().Shot();
            }
        }

        protected override void SwitchState()
        {
            if (Context._playerStatuses.attackInvoked)
            {
                if (Context._playerStatuses.isWeaponHanded)
                {
                    if (Context._playerStatuses.reloadInvoked)
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
            Context._weaponHolder.GetCurrentWeapon().InitTimeCount();
        }

        protected internal override void Update()
        {
            Context._weaponHolder.GetCurrentWeapon().Reload();
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.reloadInvoked)
            {
                if (Context._playerStatuses.attackInvoked)
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
