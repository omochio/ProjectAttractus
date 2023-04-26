using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class FallState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
        }

        protected internal override void Update()
        {
            base.Update();

            // Horizontal move
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._gamePlayInputManager.SmoothedMoveInput, Context._playerParameters.FallHorizontalAcceleration);
            Context._rb.velocity += targetVelocity;

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
