using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class AIEngineSystem : EngineSystem {
    public bool ignoreAV = false; //todo maybe remove this

    public void FixedUpdate() {
        if (flightControls == null) return;
        AdjustThrottleByVelocity();
    }

    public void OrientToDirection(Vector3 direction) {
        Vector3 localAV = transform.InverseTransformDirection(rigidbody.angularVelocity);
        Vector3 localTarget = transform.InverseTransformDirection(direction);
        float radiansToTargetYaw = Mathf.Atan2(localTarget.x, localTarget.z);
        float radiansToTargetPitch = -Mathf.Atan2(localTarget.y, localTarget.z);
        float radiansToTargetRoll = -radiansToTargetYaw + GetRollAngle();
        float turnRateRadians = turnRate * Mathf.Deg2Rad;
        float turnStep = turnRateRadians * Time.fixedDeltaTime;

        if (ignoreAV) {
            localAV.x = ComputeLocalAV(radiansToTargetPitch, localAV.x, turnRateRadians, 1f);
            localAV.y = ComputeLocalAV(radiansToTargetYaw, localAV.y, turnRateRadians, 1f);
            localAV.z = ComputeLocalAV(radiansToTargetRoll, localAV.z, turnRateRadians, 1f);
        } else {

            float quaterTurnRadius = turnRateRadians * 2f;//wut?
            if (Util.Between(-quaterTurnRadius, radiansToTargetPitch, quaterTurnRadius)) {
                localAV.x = ComputeLocalAV(radiansToTargetPitch, localAV.x, turnRateRadians, 0.5f);
            } else {
                localAV.x += Mathf.Sign(radiansToTargetPitch) * turnStep;
            }

            if (Util.Between(-quaterTurnRadius, radiansToTargetYaw, quaterTurnRadius)) {
                localAV.y = ComputeLocalAV(radiansToTargetYaw, localAV.y, turnRateRadians, 0.5f);
            } else {
                localAV.y += Mathf.Sign(radiansToTargetYaw) * turnStep;// Mathf.Clamp(radiansToTargetYaw, -1, 1);
            }

            if (Util.Between(-quaterTurnRadius, radiansToTargetRoll, quaterTurnRadius)) {
                localAV.z = ComputeLocalAV(radiansToTargetRoll, localAV.z, turnRateRadians, 0.5f);
            } else {
                localAV.z += Mathf.Sign(radiansToTargetRoll) * turnStep;// Mathf.Clamp(radiansToTargetRoll, -1, 1);
            }

        }
        localAV.x = Mathf.Clamp(localAV.x, -turnRateRadians, turnRateRadians);
        localAV.y = Mathf.Clamp(localAV.y, -turnRateRadians, turnRateRadians);
        localAV.z = Mathf.Clamp(localAV.z, -turnRateRadians, turnRateRadians);
        rigidbody.angularVelocity = transform.TransformDirection(localAV);
    }

    protected float GetRollAngle() {
        var flatForward = transform.forward;
        float rollAngle = 0f;
        flatForward.y = 0;
        if (flatForward.sqrMagnitude > 0) {
            flatForward.Normalize();
            var flatRight = Vector3.Cross(Vector3.up, flatForward);
            var localFlatRight = transform.InverseTransformDirection(flatRight);
            rollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
        }
        return rollAngle;
    }
    //Vector3 force = goalVelocity - rigidbody.velocity; this yields a really interesting corkscrew behavior, cool for evasion
}