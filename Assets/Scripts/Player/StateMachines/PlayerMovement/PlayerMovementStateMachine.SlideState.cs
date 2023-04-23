using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class SlideState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();

            float slideForce;

            if (Context._playerStatuses.isSlideCooling)
            {
                slideForce = Context._playerParameters.SmallSlideForce;
            }
            else
            {
                slideForce = Context._playerParameters.SlideForce;
            }

            Vector3 force = Context.transform.rotation
                * Context._gamePlayInputManager.SmoothedMoveInput
                * slideForce;

            Context._rb.AddForce(force, ForceMode.Impulse);

            Context._playerStatuses.SlideElapsedTime = 0f;
        }

        protected internal override void Update()
        {
            base.Update();
            Vector3 resistanceAcceleration = Vector3.Scale(Context._rb.velocity.normalized, new Vector3(-1f, 0f, -1f));
            resistanceAcceleration = Vector3.Scale(resistanceAcceleration, Context._playerParameters.SlideResistanceAcceleration);
            Context._rb.AddForce(resistanceAcceleration, ForceMode.Acceleration);
        }

        protected override void SwitchState()
        {

            if (Context._playerStatuses.jumpInvoked)
            {
                StateMachine.SendEvent(StateEvent.Jump);
            }
            else if (!Context._playerStatuses.crouchOrSlideInvoked)
            {
                if (Context._playerStatuses.moveInvoked)
                {
                    if (Context._playerStatuses.sprintInvoked)
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
            else
            {
                if (!Context._playerStatuses.isSlidable)
                {
                    StateMachine.SendEvent(StateEvent.Crouch);
                }
            }

        }
    }
}
