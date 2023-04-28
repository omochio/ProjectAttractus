using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class WalkState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
        }

        protected internal override void Update()
        {
            base.Update();
            var targetVelocity = Context.transform.rotation 
                * Vector3.Scale(Context._playerStatus.SmoothedMoveInput, Context._playerParameters.WalkSpeed);
            Context._rb.velocity = targetVelocity;
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatus.MoveInvoked)
            {
                StateMachine.SendEvent(StateEvent.Idle);
            }
            else if (Context._playerStatus.SprintInvoked)
            {
                StateMachine.SendEvent(StateEvent.Sprint);
            }
            else if (Context._playerStatus.CrouchOrSlideInvoked)
            {
                StateMachine.SendEvent(StateEvent.Crouch);
            }
            else if (Context._playerStatus.JumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
        }
    }
}
