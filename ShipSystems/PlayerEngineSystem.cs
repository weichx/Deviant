using UnityEngine;

public class PlayerEngineSystem : EngineSystem {
    public float angularDrag = 3f;

    public override void Start() {
        base.Start();
        rigidbody.angularDrag = angularDrag;
    }

    public void FixedUpdate() {
        if (flightControls == null) return;
        Vector3 localAV = transform.InverseTransformDirection(rigidbody.angularVelocity);
        float turnRateRadians = turnRate * Mathf.Deg2Rad;
        float turnStep = turnRateRadians * Time.fixedDeltaTime;

        localAV.x += flightControls.Pitch * turnStep;
        localAV.y += flightControls.Yaw  * turnStep;
        localAV.z += flightControls.Roll * turnStep;

        localAV.x = Mathf.Clamp(localAV.x, -turnRateRadians, turnRateRadians);
        localAV.y = Mathf.Clamp(localAV.y, -turnRateRadians, turnRateRadians);
        localAV.z = Mathf.Clamp(localAV.z, -turnRateRadians, turnRateRadians);

        rigidbody.angularVelocity = transform.TransformDirection(localAV);
        AdjustThrottleByVelocity();
    }

}

