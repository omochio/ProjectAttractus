using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class CrouchState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._playerStatus.SmoothedMoveInput, Context._playerParameters.CrouchSpeed);
            Context._rb.velocity = targetVelocity;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.JumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (!Context._playerStatus.CrouchOrSlideInvoked)
            {
                if (Context._playerStatus.MoveInvoked)
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
