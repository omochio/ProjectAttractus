using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;

public class PlayerStateMachine : MonoBehaviour
{
    class PlayerMovementStateBase : ImtStateMachine<PlayerStateMachine, MovementStateEvent>.State 
    {
        protected internal override void Enter()
        {
            Context._switchState = SwitchState;
        }
        protected virtual void SwitchState() { }
    }

    enum MovementStateEvent
    {
        doNothing,
        Idle,
        Walk,
        Sprint,
        Slide,
        Crouch,
        Jump,
    }

    ImtStateMachine<PlayerStateMachine, MovementStateEvent> _stateMachine;

    UnityAction _switchState;

    PlayerStatuses _playerStatuses;
    PlayerParameters _playerParameters;
    PlayerInputHandler _playerInputHandler;
    //Rigidbody _rb;
    PlayerMovementManager _playerMovementManager;
    Collider _collider;
    Animator _animator;
    PlayerAnimatorParameters _animPrams;


    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        TryGetComponent(out _playerParameters);
        //TryGetComponent(out _rb);
        TryGetComponent(out _playerMovementManager);
        TryGetComponent(out _playerInputHandler);
        TryGetComponent(out _collider);
        TryGetComponent(out _animator);
        _animPrams = new();

        _stateMachine = new ImtStateMachine<PlayerStateMachine, MovementStateEvent>(this);

        _stateMachine.SetStartState<IdleState>();

        // Any states
        _stateMachine.AddAnyTransition<DoNothingState>(MovementStateEvent.doNothing);

        //From DoNothing
        _stateMachine.AddTransition<DoNothingState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<DoNothingState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<DoNothingState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<DoNothingState, CrouchState>(MovementStateEvent.Crouch);
        _stateMachine.AddTransition<DoNothingState, SlideState>(MovementStateEvent.Slide);
        _stateMachine.AddTransition<DoNothingState, JumpState>(MovementStateEvent.Jump);

        // From Idle
        _stateMachine.AddTransition<IdleState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<IdleState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<IdleState, CrouchState>(MovementStateEvent.Crouch);
        _stateMachine.AddTransition<IdleState, JumpState>(MovementStateEvent.Jump);

        // From Walk
        _stateMachine.AddTransition<WalkState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<WalkState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<WalkState, CrouchState>(MovementStateEvent.Crouch);
        _stateMachine.AddTransition<WalkState, JumpState>(MovementStateEvent.Jump);
        
        // From Sprint
        _stateMachine.AddTransition<SprintState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<SprintState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<SprintState, SlideState>(MovementStateEvent.Slide);
        _stateMachine.AddTransition<SprintState, CrouchState>(MovementStateEvent.Crouch);
        _stateMachine.AddTransition<SprintState, JumpState>(MovementStateEvent.Jump);

        // From Slide
        _stateMachine.AddTransition<SlideState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<SlideState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<SlideState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<SlideState, CrouchState>(MovementStateEvent.Crouch);
        _stateMachine.AddTransition<SlideState, JumpState>(MovementStateEvent.Jump);
        
        // From Crouch
        _stateMachine.AddTransition<CrouchState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<CrouchState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<CrouchState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<CrouchState, JumpState>(MovementStateEvent.Jump);

        // From Jump
        _stateMachine.AddTransition<JumpState, IdleState>(MovementStateEvent.Idle);
        _stateMachine.AddTransition<JumpState, WalkState>(MovementStateEvent.Walk);
        _stateMachine.AddTransition<JumpState, SprintState>(MovementStateEvent.Sprint);
        _stateMachine.AddTransition<JumpState, SlideState>(MovementStateEvent.Slide);
        _stateMachine.AddTransition<JumpState, CrouchState>(MovementStateEvent.Crouch);
    }

    void Start()
    {
        _playerStatuses.slideElapsedTime = _playerParameters.slideCoolTime;

        _playerMovementManager.gravityAcceleration = Vector3.down * _playerParameters.gravityAcceleration;
        _playerMovementManager.lerpRate = _playerParameters.baseSpeedLerpRate;

        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _animator.SetFloat(_animPrams.animParamIDs["moveX"], _playerInputHandler.smoothedMoveInput.x);
        _animator.SetFloat(_animPrams.animParamIDs["moveY"], _playerInputHandler.smoothedMoveInput.y);

        _playerMovementManager.ApplyGravity();

        // TODO: Re consider conditions
        _playerStatuses.isGrounded = (Mathf.Abs(_playerMovementManager.GetVelocity().y) <= 0.1f) && Physics.Raycast(transform.position + Vector3.up * _collider.bounds.extents.y, Vector3.down, _collider.bounds.extents.y + 0.11f);

        Vector2 horizontalVelocity = new(
            _playerMovementManager.GetVelocity().x,
            _playerMovementManager.GetVelocity().z);
        if (horizontalVelocity.sqrMagnitude < Mathf.Pow(_playerParameters.minSlidableSpeed, 2f) || !_playerStatuses.isGrounded)
        {
            _playerStatuses.isSlidable = false;
        }
        else
        {
            _playerStatuses.isSlidable = true;
        }

        if (_playerStatuses.isSlideCooling && _playerStatuses.isGrounded)
        {
            _playerStatuses.slideElapsedTime += Time.fixedDeltaTime;
        }
        _playerStatuses.isSlideCooling = _playerStatuses.slideElapsedTime < _playerParameters.slideCoolTime;

        _stateMachine.Update();

        _playerMovementManager.ApplyVelocityChange();
        _switchState.Invoke();

        Debug.Log(_stateMachine.CurrentStateName);

    }


    class DoNothingState : PlayerMovementStateBase { }

    class IdleState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["idle"], true);
        }

        protected internal override void Update()
        {
            Context._playerMovementManager.targetVelocity = Vector3.zero;
        }

        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["idle"], false);
        }


        protected override void SwitchState()
        {
            if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Jump);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Crouch);
            }
            else if (Context._playerStatuses.moveInvoked)
            {
                if (Context._playerStatuses.sprintInvoked)
                {
                    StateMachine.SendEvent(MovementStateEvent.Sprint);
                }
                else
                {
                    StateMachine.SendEvent(MovementStateEvent.Walk);
                }
            }
        }
    }

    class WalkState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["walk"], true);
        }

        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.walkSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.walkSpeed.y);
            Context._playerMovementManager.targetVelocity = targetVelocity;
        }

        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["walk"], false);
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.moveInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Idle);
            }
            else if (Context._playerStatuses.sprintInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Sprint);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Crouch);
            }
            else if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Jump);
            }
        }
    }

    class SprintState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["sprint"], true);
        }

        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.sprintSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.sprintSpeed.y);
            Context._playerMovementManager.targetVelocity = targetVelocity;
        }

        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["sprint"], false);
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.moveInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Idle);
            }
            else if (!Context._playerStatuses.sprintInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Walk);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.isSlidable)
                {
                    StateMachine.SendEvent(MovementStateEvent.Slide);
                }
            }
            else if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Jump);
            }
        }
    }

    class CrouchState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["crouch"], true);
        }

        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.crouchSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.crouchSpeed.y);
            Context._playerMovementManager.targetVelocity = targetVelocity;
        }

        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["crouch"], false);
        }

        protected override void SwitchState()
        {
            if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Jump);
            }
            else if (!Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(MovementStateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(MovementStateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(MovementStateEvent.Idle);
                }
            }
        }
    }

    class SlideState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["slide"], true);

            float slideForce;

            if (Context._playerStatuses.isSlideCooling)
            {
                slideForce = Context._playerParameters.smallSlideForce;
            }
            else
            {
                slideForce = Context._playerParameters.slideForce;
            }

            Vector3 force = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x,
                0f,
                Context._playerInputHandler.moveInput.y) * slideForce;

            Context._playerMovementManager.AddForce(force, ForceMode.Impulse);

            Context._playerStatuses.slideElapsedTime = 0f;
        }

        protected internal override void Update()
        {
            Vector3 resistanceAcceleration = new(
                -Context._playerMovementManager.GetVelocity().normalized.x * Context._playerParameters.slideResistanceAcceleration.x,
                0f,
                -Context._playerMovementManager.GetVelocity().normalized.z * Context._playerParameters.slideResistanceAcceleration.y);

            Context._playerMovementManager.AddForce(resistanceAcceleration, ForceMode.Acceleration);
        }

        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["slide"], false);
        }

        protected override void SwitchState()
        {

            if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(MovementStateEvent.Jump);
            }
            else if (!Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(MovementStateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(MovementStateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(MovementStateEvent.Idle);
                }
            }
            else
            {
                if (!Context._playerStatuses.isSlidable)
                {
                    StateMachine.SendEvent(MovementStateEvent.Crouch);
                }
            }

        }
    }

    class JumpState : PlayerMovementStateBase
    {
        Vector3 _initVelocity;

        protected internal override void Enter()
        {
            base.Enter();
            Context._animator.SetBool(Context._animPrams.animParamIDs["jump"], true);

            _initVelocity = new(
                Context._playerMovementManager.GetVelocity().x, 
                0f, 
                Context._playerMovementManager.GetVelocity().z);
            Vector3 force = Context.transform.rotation * Vector3.up * Context._playerParameters.jumpForce;
            Context._playerMovementManager.AddForce(force, ForceMode.Impulse);
            Context._playerStatuses.isGrounded = false;
        }

        protected internal override void Update()
        {
            Vector3 input = new(
                Context._playerInputHandler.moveInput.x,
                0f,
                Context._playerInputHandler.moveInput.y);

            Vector3 targetVelocity = Context.transform.rotation * (new Vector3(
                input.x * Context._playerParameters.jumpAdditionalSpeed.x,
                0f,
                input.z * Context._playerParameters.jumpAdditionalSpeed.y) * (Mathf.Acos(Vector3.Dot(_initVelocity.normalized, Context.transform.rotation * input.normalized)) / Mathf.PI)) + Context._playerMovementManager.GetVelocity();

            Context._playerMovementManager.targetVelocity = targetVelocity;
        }


        protected internal override void Exit()
        {
            Context._animator.SetBool(Context._animPrams.animParamIDs["jump"], false);

            Context._playerStatuses.jumpInvoked = false;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatuses.isGrounded)
            {
                if (Context._playerStatuses.crouchOrSlideInvoked)
                {
                    if (Context._playerStatuses.isSlidable)
                    {
                        StateMachine.SendEvent(MovementStateEvent.Slide);
                    }
                    else
                    {
                        StateMachine.SendEvent(MovementStateEvent.Crouch);
                    }
                }
                else if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(MovementStateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(MovementStateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(MovementStateEvent.Idle);
                }
            }
        }
    }
}
