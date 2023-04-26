using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;

public partial class PlayerMovementStateMachine : MonoBehaviour
{
    class PlayerMovementStateBase : ImtStateMachine<PlayerMovementStateMachine, StateEvent>.State
    {
        protected internal override void Enter()
        {
            Context._switchState = SwitchState;
        }

        protected internal override void Update()
        {
            // TODO: Re consider conditions
            Context._playerStatus.isGrounded = (Mathf.Abs(Context._rb.velocity.y) <= 0.1f) && Physics.Raycast(Context.transform.position + Vector3.up * Context._collider.bounds.extents.y, Vector3.down, Context._collider.bounds.extents.y + 0.11f);

            Vector2 horizontalVelocity = new(
                Context._rb.velocity.x,
                Context._rb.velocity.z);
            if (horizontalVelocity.sqrMagnitude < Mathf.Pow(Context._playerParameters.MinSlidableSpeed, 2f) || !Context._playerStatus.isGrounded)
            {
                Context._playerStatus.isSlidable = false;
            }
            else
            {
                Context._playerStatus.isSlidable = true;
            }

            if (Context._playerStatus.isSlideCooling && Context._playerStatus.isGrounded)
            {
                Context._playerStatus.SlideElapsedTime += Time.fixedDeltaTime;
            }
            Context._playerStatus.isSlideCooling = Context._playerStatus.SlideElapsedTime < Context._playerParameters.SlideCoolTime;
        }

        protected virtual void SwitchState() { }
    }

    enum StateEvent
    {
        Idle,
        Walk,
        Sprint,
        Slide,
        Crouch,
        Jump,
        AtraForce,
        Fall
    }

    ImtStateMachine<PlayerMovementStateMachine, StateEvent> _stateMachine;

    UnityAction _switchState;

    PlayerStatus _playerStatus;
    GamePlayInputManager _gamePlayInputManager;
    Collider _collider;
    Rigidbody _rb;
    [SerializeField]
    PlayerParameters _playerParameters;
    [SerializeField]
    AtraGunHolder _atraGunHolder;



    void Awake()
    {
        TryGetComponent(out _playerStatus);
        TryGetComponent(out _gamePlayInputManager);
        TryGetComponent(out _collider);
        TryGetComponent(out _rb);

        // Initialize support classes
        _playerStatus.SlideElapsedTime = _playerParameters.SlideCoolTime;

        _stateMachine = new ImtStateMachine<PlayerMovementStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<IdleState>();

        // From AtraForce
        _stateMachine.AddTransition<AtraForceState, FallState>(StateEvent.Idle);
        _stateMachine.AddTransition<AtraForceState, FallState>(StateEvent.Fall);

        // From Fall
        _stateMachine.AddTransition<FallState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<FallState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<FallState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<FallState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<FallState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<FallState, AtraForceState>(StateEvent.AtraForce);

        // From Idle
        _stateMachine.AddTransition<IdleState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<IdleState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<IdleState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<IdleState, JumpState>(StateEvent.Jump);
        _stateMachine.AddTransition<IdleState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<IdleState, FallState>(StateEvent.Fall);

        // From Walk
        _stateMachine.AddTransition<WalkState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<WalkState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<WalkState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<WalkState, JumpState>(StateEvent.Jump);
        _stateMachine.AddTransition<WalkState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<WalkState, FallState>(StateEvent.Fall);

        // From Sprint
        _stateMachine.AddTransition<SprintState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<SprintState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<SprintState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<SprintState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<SprintState, JumpState>(StateEvent.Jump);
        _stateMachine.AddTransition<SprintState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<SprintState, FallState>(StateEvent.Fall);

        // From Slide
        _stateMachine.AddTransition<SlideState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<SlideState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<SlideState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<SlideState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<SlideState, JumpState>(StateEvent.Jump);
        _stateMachine.AddTransition<SlideState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<SlideState, FallState>(StateEvent.Fall);

        // From Crouch
        _stateMachine.AddTransition<CrouchState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<CrouchState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<CrouchState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<CrouchState, JumpState>(StateEvent.Jump);
        _stateMachine.AddTransition<CrouchState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<CrouchState, FallState>(StateEvent.Fall);

        // From Jump
        _stateMachine.AddTransition<JumpState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<JumpState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<JumpState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<JumpState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<JumpState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<JumpState, AtraForceState>(StateEvent.AtraForce);
        _stateMachine.AddTransition<JumpState, FallState>(StateEvent.Fall);
    }

    public void UpdateState()
    {
        _stateMachine.Update();
        //Debug.Log(_stateMachine.CurrentStateName);
        //Debug.Log(_playerStatuses.isSlidable);
    }

    public void SwitchState()
    {
        if (_playerStatus.isAtraForceEnabled)
        {
            _stateMachine.SendEvent(StateEvent.AtraForce);
        }
        else if (_stateMachine.CurrentStateName != "JumpState" && !_playerStatus.isGrounded)
        {
            _stateMachine.SendEvent(StateEvent.Fall);
        }
        _switchState.Invoke();
    }
}
