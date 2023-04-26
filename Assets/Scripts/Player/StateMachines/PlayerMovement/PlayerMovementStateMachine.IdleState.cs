using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class IdleState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            Context._rb.velocity = Vector3.zero;
        }

        protected override void SwitchState()
        {
            if (Context._playerStatus.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (Context._playerStatus.crouchOrSlideInvoked)
            {
                StateMachine.SendEvent(StateEvent.Crouch);
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
        }
    }
}
