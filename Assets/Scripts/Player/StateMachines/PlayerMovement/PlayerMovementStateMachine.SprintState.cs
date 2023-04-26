using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class SprintState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            base.Update();
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._gamePlayInputManager.SmoothedMoveInput, Context._playerParameters.SprintSpeed);
            Context._rb.velocity = targetVelocity;
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatus.moveInvoked)
            {
                StateMachine.SendEvent(StateEvent.Idle);
            }
            else if (!Context._playerStatus.sprintInvoked)
            {
                StateMachine.SendEvent(StateEvent.Walk);
            }
            else if (Context._playerStatus.crouchOrSlideInvoked)
            {
                if (Context._playerStatus.isSlidable)
                {
                    StateMachine.SendEvent(StateEvent.Slide);
                }
            }
            else if (Context._playerStatus.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
        }
    }
}
