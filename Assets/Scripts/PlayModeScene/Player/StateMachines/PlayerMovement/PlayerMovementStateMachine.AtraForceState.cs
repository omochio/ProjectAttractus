using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class AtraForceState : PlayerMovementStateBase
    {
        protected internal override void Enter()
        {
            base.Enter();
            Context._playerStatus.IsGravityEnabled = false;
        }

        protected internal override void Update()
        {
            base.Update();

            // Apply Atra force
            Context._atraGunHolder.GetCurrentAtraGun().AddAtraForce(Context.transform, Context._rb);

            // Horizontal move
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._playerStatus.SmoothedMoveInput, Context._playerParameters.AtraForceHorizontalAcceleration);
            Context._rb.velocity += targetVelocity;
        }

        protected internal override void Exit()
        {
            Context._playerStatus.IsGravityEnabled = true;
        }

        protected override void SwitchState()
        {
            if (!Context._playerStatus.IsAtraForceEnabled)
            {
                stateMachine.SendEvent(StateEvent.Fall);
            }
        }
    }
}
