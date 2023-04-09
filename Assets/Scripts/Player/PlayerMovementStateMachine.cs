using UnityEngine;
using IceMilkTea.Core;

public class PlayerMovementStateMachine : MonoBehaviour
{
    class PlayerMovementStateBase : ImtStateMachine<PlayerMovementStateMachine, MovementStateEvent>.State { }

    public enum MovementStateEvent
    {
        doNothing,
        Idle,
        Walk,
        Sprint,
        Slide,
        Crouch,
        Jump,
    }

    ImtStateMachine<PlayerMovementStateMachine, MovementStateEvent> _stateMachine;
    public ImtStateMachine<PlayerMovementStateMachine, MovementStateEvent> stateMachine
    {
        get { return _stateMachine; }
    }

    bool _isTransitionable = true;
    public bool isTransitionable
    {
        get { return _isTransitionable; }
        set { _isTransitionable = value; }
    }

    PlayerStatuses _playerStatuses;
    PlayerParameters _playerParameters;
    PlayerInputHandler _playerInputHandler;
    Rigidbody _rb;
    Collider _collider;


    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        TryGetComponent(out _playerParameters);
        TryGetComponent(out _rb);
        TryGetComponent(out _playerInputHandler);
        TryGetComponent(out _collider);

        _stateMachine = new ImtStateMachine<PlayerMovementStateMachine, MovementStateEvent>(this);

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

        _stateMachine.Update();
    }

    void FixedUpdate()
    {

        _rb.AddForce(Vector3.down * _playerParameters.gravityAcceleration, ForceMode.Acceleration);

        _playerStatuses.isGrounded = (Mathf.Abs(_rb.velocity.y) <= 0.1f) && Physics.Raycast(transform.position + Vector3.up * _collider.bounds.extents.y, Vector3.down, _collider.bounds.extents.y + 0.11f);

        if (_rb.velocity.sqrMagnitude < _playerParameters.crouchSpeed.sqrMagnitude || !_playerStatuses.isGrounded)
        {
            _playerStatuses.isSlidable = false;
        }
        else if (_rb.velocity.sqrMagnitude > Mathf.Pow(_playerParameters.minSlidableSpeed, 2f))
        {
            _playerStatuses.isSlidable = true;
        }

        if (_playerStatuses.isSlideCooling && _playerStatuses.isGrounded)
        {
            _playerStatuses.slideElapsedTime += Time.fixedDeltaTime;
        }
        _playerStatuses.isSlideCooling = _playerStatuses.slideElapsedTime < _playerParameters.slideCoolTime;

        _stateMachine.Update();

        Debug.Log(_stateMachine.CurrentStateName);

    }


    class DoNothingState : PlayerMovementStateBase { }

    class IdleState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            Context._rb.velocity = Utilities.FRILerp(Context._rb.velocity, Vector3.zero, Context._playerParameters.baseSpeedLerpRate, Time.fixedDeltaTime);

            SwitchStateByInput();
        }

        void SwitchStateByInput()
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
        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.walkSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.walkSpeed.y);
            Context._rb.velocity = Utilities.FRILerp(Context._rb.velocity, targetVelocity, Context._playerParameters.baseSpeedLerpRate, Time.fixedDeltaTime);

            SwitchStateByInput();
        }

        void SwitchStateByInput()
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
        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.sprintSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.sprintSpeed.y);
            Context._rb.velocity = Utilities.FRILerp(Context._rb.velocity, targetVelocity, Context._playerParameters.baseSpeedLerpRate, Time.fixedDeltaTime);
            
            SwitchStateByInput();
        }

        void SwitchStateByInput()
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
        protected internal override void Update()
        {
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.moveInput.x * Context._playerParameters.crouchSpeed.x,
                0f,
                Context._playerInputHandler.moveInput.y * Context._playerParameters.crouchSpeed.y);
            Context._rb.velocity = Utilities.FRILerp(Context._rb.velocity, targetVelocity, Context._playerParameters.baseSpeedLerpRate, Time.fixedDeltaTime);

            SwitchState();
        }

        void SwitchState()
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

            Context._rb.AddForce(force, ForceMode.Impulse);

            Context._playerStatuses.slideElapsedTime = 0f;
        }

        protected internal override void Update()
        {
            Vector3 resistanceAcceleration = new(
                -Context._rb.velocity.normalized.x * Context._playerParameters.slideResistanceAcceleration.x,
                0f,
                -Context._rb.velocity.normalized.z * Context._playerParameters.slideResistanceAcceleration.y);

            Context._rb.AddForce(resistanceAcceleration, ForceMode.Acceleration);

            SwitchState();
        }

        void SwitchState()
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
            _initVelocity = new(
                Context._rb.velocity.x, 
                0f, 
                Context._rb.velocity.z);
            Vector3 force = Context.transform.rotation * Vector3.up * Context._playerParameters.jumpForce;
            Context._rb.AddForce(force, ForceMode.Impulse);
            Context._playerStatuses.isGrounded = false;
        }

        protected internal override void Update()
        {
            Vector3 input = new(
                Context._playerInputHandler.moveInput.x,
                0f,
                Context._playerInputHandler.moveInput.y);

            //Vector3 horizontalVelocity = new(
            //    Context._rb.velocity.x,
            //    0f,
            //    Context._rb.velocity.z);

            Vector3 targetVelocity = Context.transform.rotation * (new Vector3(
                input.x * Context._playerParameters.jumpAdditionalSpeed.x,
                0f,
                input.z * Context._playerParameters.jumpAdditionalSpeed.y) * (Mathf.Acos(Vector3.Dot(_initVelocity.normalized, Context.transform.rotation * input.normalized)) / Mathf.PI)) + Context._rb.velocity;

            Context._rb.velocity = Utilities.FRILerp(Context._rb.velocity, targetVelocity, Context._playerParameters.baseSpeedLerpRate, Time.fixedDeltaTime);

            if (Context._playerStatuses.isGrounded)
            {
                SwitchState();
            }
        }


        protected internal override void Exit()
        {
            Context._playerStatuses.jumpInvoked = false;
        }

        void SwitchState()
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
