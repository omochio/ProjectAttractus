using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class SlideState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();

            float slideForce;

            if (Context._playerStatus.isSlideCooling)
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

            Context._playerStatus.SlideElapsedTime = 0f;
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
            else
            {
                if (!Context._playerStatus.isSlidable)
                {
                    StateMachine.SendEvent(StateEvent.Crouch);
                }
            }

        }
    }
}
