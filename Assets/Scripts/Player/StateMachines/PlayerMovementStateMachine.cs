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
            Context._playerMovementManager.ApplyGravity();

            // TODO: Re consider conditions
            Context._playerStatuses.isGrounded = (Mathf.Abs(Context._playerMovementManager.GetVelocity().y) <= 0.1f) && Physics.Raycast(Context.transform.position + Vector3.up * Context._collider.bounds.extents.y, Vector3.down, Context._collider.bounds.extents.y + 0.11f);

            Vector2 horizontalVelocity = new(
                Context._playerMovementManager.GetVelocity().x,
                Context._playerMovementManager.GetVelocity().z);
            if (horizontalVelocity.sqrMagnitude < Mathf.Pow(Context._playerParameters.MinSlidableSpeed, 2f) || !Context._playerStatuses.isGrounded)
            {
                Context._playerStatuses.isSlidable = false;
            }
            else
            {
                Context._playerStatuses.isSlidable = true;
            }

            if (Context._playerStatuses.isSlideCooling && Context._playerStatuses.isGrounded)
            {
                Context._playerStatuses.SlideElapsedTime += Time.fixedDeltaTime;
            }
            Context._playerStatuses.isSlideCooling = Context._playerStatuses.SlideElapsedTime < Context._playerParameters.SlideCoolTime;
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

    PlayerStatuses _playerStatuses;
    PlayerParameters _playerParameters;
    PlayerInputHandler _playerInputHandler;
    PlayerMovementManager _playerMovementManager;
    Collider _collider;
    [SerializeField]
    AtraGunHolder _atraGunHolder;


    void Awake()
    {
        TryGetComponent(out _playerStatuses);
        TryGetComponent(out _playerParameters);
        TryGetComponent(out _playerMovementManager);
        TryGetComponent(out _playerInputHandler);
        TryGetComponent(out _collider);


        // Initialize support classes
        _playerStatuses.SlideElapsedTime = _playerParameters.SlideCoolTime;
        _playerMovementManager.GravityAcceleration = Vector3.down * _playerParameters.GravityAcceleration;
        _playerMovementManager.LerpRate = _playerParameters.BasicSpeedLerpRate;


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
        Debug.Log(_stateMachine.CurrentStateName);
    }

    public void SwitchState()
    {
        if (_playerStatuses.isAtraForceEnabled)
        {
            _stateMachine.SendEvent(StateEvent.AtraForce);
        }
        else if (_stateMachine.CurrentStateName != "JumpState" && !_playerStatuses.isGrounded)
        {
            _stateMachine.SendEvent(StateEvent.Fall);
        }
        _switchState.Invoke();
    }


    //class DoNothingState : PlayerMovementStateBase { }

    class IdleState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
        }

        protected internal override void Update()
        {
            Context._playerMovementManager.TargetVelocity = Vector3.zero;
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
        }

        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.MoveInput.x * Context._playerParameters.WalkSpeed.x,
                0f,
                Context._playerInputHandler.MoveInput.y * Context._playerParameters.WalkSpeed.y);
            Context._playerMovementManager.TargetVelocity = targetVelocity;
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
        }

        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.MoveInput.x * Context._playerParameters.SprintSpeed.x,
                0f,
                Context._playerInputHandler.MoveInput.y * Context._playerParameters.SprintSpeed.y);
            Context._playerMovementManager.TargetVelocity = targetVelocity;
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
        }

        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.MoveInput.x * Context._playerParameters.CrouchSpeed.x,
                0f,
                Context._playerInputHandler.MoveInput.y * Context._playerParameters.CrouchSpeed.y);
            Context._playerMovementManager.TargetVelocity = targetVelocity;
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

            float slideForce;

            if (Context._playerStatuses.isSlideCooling)
            {
                slideForce = Context._playerParameters.SmallSlideForce;
            }
            else
            {
                slideForce = Context._playerParameters.SlideForce;
            }

            Vector3 force = Context.transform.rotation * new Vector3(
                Context._playerInputHandler.MoveInput.x,
                0f,
                Context._playerInputHandler.MoveInput.y) * slideForce;

            Context._playerMovementManager.AddForce(force, ForceMode.Impulse);

            Context._playerStatuses.SlideElapsedTime = 0f;
        }

        protected internal override void Update()
        {
            base.Update();
            // TODO: Resistance acceleration is not properly applied
            Vector3 resistanceAcceleration = new(
                -Context._playerMovementManager.GetVelocity().normalized.x * Context._playerParameters.SlideResistanceAcceleration.x,
                0f,
                -Context._playerMovementManager.GetVelocity().normalized.z * Context._playerParameters.SlideResistanceAcceleration.y);
            Context._playerMovementManager.AddForce(resistanceAcceleration, ForceMode.Acceleration);
            //Debug.Log(Context._playerMovementManager.GetVelocity().normalized);
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
        //Vector3 _initVelocity;

        protected internal override void Enter()
        {
            base.Enter();

            //_initVelocity = new(
            //    Context._playerMovementManager.GetVelocity().x, 
            //    0f, 
            //    Context._playerMovementManager.GetVelocity().z);
            Vector3 force = Context.transform.rotation * Vector3.up * Context._playerParameters.JumpForce;
            Context._playerMovementManager.AddForce(force, ForceMode.Impulse);
            Context._playerStatuses.isGrounded = false;
        }

        protected internal override void Update()
        {
            base.Update();
            Vector3 input = new(
                Context._playerInputHandler.MoveInput.x,
                0f,
                Context._playerInputHandler.MoveInput.y);

            Vector3 targetVelocity = Context.transform.rotation * (new Vector3(
                input.x * Context._playerParameters.JumpHorizontalSpeed.x,
                0f,
                input.z * Context._playerParameters.JumpHorizontalSpeed.y)) + Context._playerMovementManager.GetVelocity();

            Context._playerMovementManager.TargetVelocity = targetVelocity;
        }


        protected internal override void Exit()
        {
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
    
    class AtraForceState : PlayerMovementStateBase
    {
        //protected internal override void Enter()
        //{
        //    base.Enter();
        //}

        protected internal override void Update()
        {
            //Debug.Log("AtraStae");
            base.Update();

            // Apply Atra force
            Context._atraGunHolder.GetCurrentAtraGun().AddAtraForce(Context.transform, Context._playerMovementManager);

            // Horizontal move
            Vector3 input = new(
                Context._playerInputHandler.MoveInput.x,
                0f,
                Context._playerInputHandler.MoveInput.y);

            Vector3 targetVelocity = Context.transform.rotation * (new Vector3(
                input.x * Context._playerParameters.AtraForceHorizontalSpeed.x,
                0f,
                input.z * Context._playerParameters.AtraForceHorizontalSpeed.y)) + Context._playerMovementManager.GetVelocity();

            Context._playerMovementManager.TargetVelocity = targetVelocity;

        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.isAtraForceEnabled)
            {
                stateMachine.SendEvent(StateEvent.Fall);
            }
        }
    }

    class FallState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
        }

        protected internal override void Update()
        {
            base.Update();
            Debug.Log(Context._playerMovementManager.GetVelocity());
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
