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
                * Vector3.Scale(Context._playerStatus.SmoothedMoveInput, Context._playerParameters.FallHorizontalAcceleration);
            Context._rb.velocity += targetVelocity;

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
