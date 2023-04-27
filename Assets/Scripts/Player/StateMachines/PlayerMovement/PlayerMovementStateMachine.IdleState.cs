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
            if (Context._playerStatus.JumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (Context._playerStatus.CrouchOrSlideInvoked)
            {
                StateMachine.SendEvent(StateEvent.Crouch);
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
        }
    }
}
