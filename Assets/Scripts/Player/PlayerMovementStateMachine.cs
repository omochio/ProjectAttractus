using UnityEngine;
using IceMilkTea.Core;
using UnityEngine.Events;

public class PlayerMovementStateMachine : MonoBehaviour
{
    class PlayerMovementStateBase : ImtStateMachine<PlayerMovementStateMachine, StateEvent>.State 
    {
        protected internal override void Enter()
        {
            Context._switchState = SwitchState;
        }

        protected internal override void Update()
        {
            Context._animator.SetFloat(Context._animPrams.animParamIDs["moveX"], Context._playerInputHandler.smoothedMoveInput.x);
            Context._animator.SetFloat(Context._animPrams.animParamIDs["moveY"], Context._playerInputHandler.smoothedMoveInput.y);

            Context._playerMovementManager.ApplyGravity();

            // TODO: Re consider conditions
            Context._playerStatuses.isGrounded = (Mathf.Abs(Context._playerMovementManager.GetVelocity().y) <= 0.1f) && Physics.Raycast(Context.transform.position + Vector3.up * Context._collider.bounds.extents.y, Vector3.down, Context._collider.bounds.extents.y + 0.11f);

            Vector2 horizontalVelocity = new(
                Context._playerMovementManager.GetVelocity().x,
                Context._playerMovementManager.GetVelocity().z);
            if (horizontalVelocity.sqrMagnitude < Mathf.Pow(Context._playerParameters.minSlidableSpeed, 2f) || !Context._playerStatuses.isGrounded)
            {
                Context._playerStatuses.isSlidable = false;
            }
            else
            {
                Context._playerStatuses.isSlidable = true;
            }

            if (Context._playerStatuses.isSlideCooling && Context._playerStatuses.isGrounded)
            {
                Context._playerStatuses.slideElapsedTime += Time.fixedDeltaTime;
            }
            Context._playerStatuses.isSlideCooling = Context._playerStatuses.slideElapsedTime < Context._playerParameters.slideCoolTime;
        }

        protected virtual void SwitchState() { }
    }

    enum StateEvent
    {
        doNothing,
        Idle,
        Walk,
        Sprint,
        Slide,
        Crouch,
        Jump,
    }

    ImtStateMachine<PlayerMovementStateMachine, StateEvent> _stateMachine;

    UnityAction _switchState;

    PlayerStatuses _playerStatuses;
    PlayerParameters _playerParameters;
    PlayerInputHandler _playerInputHandler;
    PlayerMovementManager _playerMovementManager;
    Collider _collider;
    Animator _animator;
    PlayerAnimatorParameters _animPrams;


    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        TryGetComponent(out _playerParameters);
        TryGetComponent(out _playerMovementManager);
        TryGetComponent(out _playerInputHandler);
        TryGetComponent(out _collider);
        TryGetComponent(out _animator);
        _animPrams = new();


        // Initialize support classes
        _playerStatuses.slideElapsedTime = _playerParameters.slideCoolTime;
        _playerMovementManager.gravityAcceleration = Vector3.down * _playerParameters.gravityAcceleration;
        _playerMovementManager.lerpRate = _playerParameters.baseSpeedLerpRate;


        _stateMachine = new ImtStateMachine<PlayerMovementStateMachine, StateEvent>(this);

        _stateMachine.SetStartState<IdleState>();

        // Any states
        _stateMachine.AddAnyTransition<DoNothingState>(StateEvent.doNothing);

        // From DoNothing
        _stateMachine.AddTransition<DoNothingState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<DoNothingState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<DoNothingState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<DoNothingState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<DoNothingState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<DoNothingState, JumpState>(StateEvent.Jump);

        // From Idle
        _stateMachine.AddTransition<IdleState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<IdleState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<IdleState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<IdleState, JumpState>(StateEvent.Jump);

        // From Walk
        _stateMachine.AddTransition<WalkState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<WalkState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<WalkState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<WalkState, JumpState>(StateEvent.Jump);
        
        // From Sprint
        _stateMachine.AddTransition<SprintState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<SprintState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<SprintState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<SprintState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<SprintState, JumpState>(StateEvent.Jump);

        // From Slide
        _stateMachine.AddTransition<SlideState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<SlideState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<SlideState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<SlideState, CrouchState>(StateEvent.Crouch);
        _stateMachine.AddTransition<SlideState, JumpState>(StateEvent.Jump);
        
        // From Crouch
        _stateMachine.AddTransition<CrouchState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<CrouchState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<CrouchState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<CrouchState, JumpState>(StateEvent.Jump);

        // From Jump
        _stateMachine.AddTransition<JumpState, IdleState>(StateEvent.Idle);
        _stateMachine.AddTransition<JumpState, WalkState>(StateEvent.Walk);
        _stateMachine.AddTransition<JumpState, SprintState>(StateEvent.Sprint);
        _stateMachine.AddTransition<JumpState, SlideState>(StateEvent.Slide);
        _stateMachine.AddTransition<JumpState, CrouchState>(StateEvent.Crouch);
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
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                StateMachine.SendEvent(StateEvent.Crouch);
            }
            else if (Context._playerStatuses.moveInvoked)
            {
                if (Context._playerStatuses.sprintInvoked)
                {
                    StateMachine.SendEvent(StateEvent.Sprint);
                }
                else
                {
                    StateMachine.SendEvent(StateEvent.Walk);
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
            base.Update();
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
                StateMachine.SendEvent(StateEvent.Idle);
            }
            else if (Context._playerStatuses.sprintInvoked)
            {
                StateMachine.SendEvent(StateEvent.Sprint);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                StateMachine.SendEvent(StateEvent.Crouch);
            }
            else if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
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
            base.Update();
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
                StateMachine.SendEvent(StateEvent.Idle);
            }
            else if (!Context._playerStatuses.sprintInvoked)
            {
                StateMachine.SendEvent(StateEvent.Walk);
            }
            else if (Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.isSlidable)
                {
                    StateMachine.SendEvent(StateEvent.Slide);
                }
            }
            else if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
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
            base.Update();
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
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (!Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(StateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(StateEvent.Idle);
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
            base.Update();
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
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (!Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(StateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(StateEvent.Idle);
                }
            }
            else
            {
                if (!Context._playerStatuses.isSlidable)
                {
                    StateMachine.SendEvent(StateEvent.Crouch);
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
            base.Update();
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
                        StateMachine.SendEvent(StateEvent.Slide);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Crouch);
                    }
                }
                else if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
                    {
                        StateMachine.SendEvent(StateEvent.Sprint);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Walk);
                    }
                }
                else
                {
                    StateMachine.SendEvent(StateEvent.Idle);
                }
            }
        }
    }
}
