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
            Context._playerStatus.isGrounded = false;
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
            Context._playerStatus.jumpInvoked = false;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.isGrounded)
            {
                if (Context._playerStatus.crouchOrSlideInvoked)
                {
                    if (Context._playerStatus.isSlidable)
                    {
                        StateMachine.SendEvent(StateEvent.Slide);
                    }
                    else
                    {
                        StateMachine.SendEvent(StateEvent.Crouch);
                    }
                }
                else if (Context._playerStatus.moveInvoked)
                {
                    if (Context._playerStatus.sprintInvoked)
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
