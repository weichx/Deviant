using UnityEngine;

public class FlightControls {

    protected float yaw;
    protected float pitch;
    protected float roll;

    public float throttle;
    public float rollOverride;
    public float yawModifier;
    public float rollModifier;
    public float pitchModifer;
    public Vector3 destination; 

    public FlightControls(Vector3 destination) {
        this.destination = destination;
        SetStickDamping(1, 1, 1);
        throttle = 0f;
        rollOverride = 0f;
    }

    public FlightControls(float yawModifier = 1f, float pitchModifer = 1f, float rollModifier = 1f) {
        SetStickDamping(yawModifier, pitchModifer, rollModifier);
        throttle = 0f;
        rollOverride = 0f;
    }

    public float Yaw {
        get { return yaw * yawModifier; }
    }

    public float Pitch {
        get { return pitch * pitchModifer; }  
    }

    public float Roll {
        get { return roll * rollModifier; }
    }

    public void Decelerate(float amount = 0.1f) {
        SetThrottle(throttle - amount);
    }

    public void Accelerate(float amount = 0.1f) {
        SetThrottle(throttle + amount);
    }

    public void SetStickDamping(float yawDamping, float pitchDamping, float rollDamping) {
        yawModifier = yawDamping;
        pitchModifer = pitchDamping;
        rollModifier = rollDamping;
    }

    public void SetStickInputs(float yaw, float pitch, float roll, float throttle = 1f) {
        this.yaw = Mathf.Clamp(yaw, -1, 1);
        this.pitch = Mathf.Clamp(pitch, -1, 1);
        this.roll = Mathf.Clamp(roll, -1, 1);
        this.throttle = Mathf.Clamp01(throttle);
    }

    public void SetThrottle(float throttle) {
        this.throttle = Mathf.Clamp01(throttle);
    }
     
    public void GoTo(Vector3 destination, float throttle = 1f, float rollOverride = 0) {
        this.throttle = throttle;
        this.rollOverride = rollOverride;
        this.destination = destination;
    }

    public void TurnTo(Vector3 lookAt, float rollOverride = 0) {
        this.rollOverride = rollOverride;
        this.destination = lookAt;
    }
}