﻿using UnityEngine;

public partial class PlayerMovementStateMachine
{
    class AtraForceState : PlayerMovementStateBase
    {
        protected internal override void Update()
        {
            base.Update();

            // Apply Atra force
            Context._atraGunHolder.GetCurrentAtraGun().AddAtraForce(Context.transform, Context._rb);

            // Horizontal move
            Vector3 targetVelocity = Context.transform.rotation
                * Vector3.Scale(Context._gamePlayInputManager.SmoothedMoveInput, Context._playerParameters.AtraForceHorizontalAcceleration);
            Context._rb.velocity += targetVelocity;

        }

        protected override void SwitchState()
        {
            if (!Context._playerStatuses.isAtraForceEnabled)
            {
                stateMachine.SendEvent(StateEvent.Fall);
            }
        }
    }
}
