using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class JumpState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();

            Vector3 force = Context.transform.rotation * Vector3.up * Context._playerParameters.JumpForce;
            Context._rb.AddForce(force, ForceMode.Impulse);
            Context._playerStatus.IsGrounded = false;
        }

        protected internal override void Update()
        {
            base.Update();

            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._gamePlayInputManager.SmoothedMoveInput, Context._playerParameters.JumpHorizontalAcceleration);
            Context._rb.velocity += targetVelocity;
        }


        protected internal override void Exit()
        {
            Context._playerStatus.JumpInvoked = false;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.IsGrounded)
            {
                if (Context._playerStatus.CrouchOrSlideInvoked)
                {
                    if (Context._playerStatus.IsSlidable)
                    {
                        StateMachine.SendEvent(StateEvent.Slide);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Crouch);
                    }
                }
                else if (Context._playerStatus.MoveInvoked)
                {
                    if (Context._playerStatus.SprintInvoked)
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
