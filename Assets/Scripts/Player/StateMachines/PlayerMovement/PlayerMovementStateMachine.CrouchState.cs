using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class CrouchState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._gamePlayInputManager.SmoothedMoveInput, Context._playerParameters.CrouchSpeed);
            Context._rb.velocity = targetVelocity;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (!Context._playerStatus.crouchOrSlideInvoked)
            {
                if (Context._playerStatus.moveInvoked)
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
