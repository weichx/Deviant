using UnityEngine;

/*
Spline Mode
    Auto -- Follow spline-defined parameters for acceleration / orientation / arrival
    Arrive -- Follow spline and arrive at end of spline, used for docking probably
    Manual -- Follow spline with AI defined acceleration and orientation -- custom maneuvers
    Loose -- Follow spline & avoid things, this is used for patrol and evade

MovementModes
    TurnTowards -- no explicit speed adjustments past AI input
    ArriveAt -- will stop at point, AI sets preferred speed (or top speed)
    SlipTowoards     
    
Formation Movement -- use Arrive at control mode
        Adjusting arrivalDecelerationScale between 0.1 & 1 will scale the tightness to formation point
        Leader should move at 60% of slowest formation member speed && turn at 70% of least agile formation member turn rate
        Formation members should have a reduced avoidance to each other
        When leader is turning, slightly reduce arrivalDecelerationScale if it is over 0.6 to give formation members time to align before overshooting formation node
*/

[RequireComponent(typeof(Rigidbody))]
public class EngineSystem : MonoBehaviour {

    public new Rigidbody rigidbody;
    protected FlightControls flightControls;

    public float maxSpeed = 20f;
    public float accelerationRate = 0.25f;
    public float turnRate = 90f; // degrees per second
    public float speed;
    //   public float areoFactor = 1f;
    //   public float slip = 0.75f;
    protected float uncappedMaxSpeed;
    protected float cappedMaxSpeed;

    [SerializeField]
    protected float desiredSpeed;

    public bool IsSpeedCapped {
        get { return maxSpeed != cappedMaxSpeed; }
    }

    public float MaxSpeed {
        get { return cappedMaxSpeed; }
        set { cappedMaxSpeed = Mathf.Clamp(value, 0, maxSpeed); }
    }

    public float TurnRate_Radians {
        get { return turnRate * Mathf.Deg2Rad; }
    }

    public float TurnRate_Degrees {
        get { return turnRate; }
    }

    public bool Accelerating {
        get { return desiredSpeed > speed; }
    }

    public bool Decelerating {
        get { return desiredSpeed < speed; }
    }

    public float Power {
        get { return speed / MaxSpeed; }
    }

    public virtual void Start() {
        cappedMaxSpeed = maxSpeed;
        rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        rigidbody.angularDrag =  1.5f;
        rigidbody.useGravity = false;
    }

    public void SetFlightControls(FlightControls flightControls) {
        this.flightControls = flightControls;
    }

    //todo this is currently unused, find a use for it
    //protected void AdjustThrottleByForce() {
    //    var localVelocity = transform.InverseTransformDirection(rigidbody.velocity);
    //    float ForwardSpeed = Mathf.Max(0, localVelocity.z);

    //    rigidbody.AddForce(MaxSpeed * flightControls.throttle * transform.forward, ForceMode.Acceleration);
    //    if (rigidbody.velocity.sqrMagnitude > 0) {
    //        // compare the direction we're pointing with the direction we're moving
    //        //areoFactor is between 1 and -1
    //        areoFactor = Vector3.Dot(transform.forward, rigidbody.velocity.normalized);
    //        // multipled by itself results in a desirable rolloff curve of the effect
    //        areoFactor *= areoFactor;
    //        // Finally we calculate a new velocity by bending the current velocity direction towards
    //        // the the direction the plane is facing, by an amount based on this aeroFactor
    //        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, transform.forward * ForwardSpeed, areoFactor * ForwardSpeed * slip * Time.deltaTime);
    //    }
    //    rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, MaxSpeed);
    //    speed = rigidbody.velocity.magnitude;
    //}

    protected void AdjustThrottleByVelocity() {
        //acceleration rate -- max percentage of max speed per second we can accelerate
        desiredSpeed = MaxSpeed * flightControls.throttle;
        float speedStep = accelerationRate * MaxSpeed * Time.fixedDeltaTime;

        if (speed > desiredSpeed) {
            speedStep *= -1;
        }

        speed = Mathf.Clamp(speed + speedStep, 0, MaxSpeed);
        rigidbody.velocity = transform.forward * speed;
    }

    //calculates force needed across a single local angular velocity axis to align with a target. Away handles current AV direction being away from desired AV
    private float Away(float inputVelocity, float maxVelocity, float radiansToGoal, float maxAcceleration, float deltaTime, bool noOvershoot) {
        if ((-inputVelocity < 1e-5) && (radiansToGoal < 1e-5)) {
            return 0f;
        }

        if (maxAcceleration == 0) {
            return inputVelocity;
        }

        float t0 = -inputVelocity / maxAcceleration; //time until velocity is zero

        if (t0 > deltaTime) {	// no reversal in this time interval
            return inputVelocity + maxAcceleration * deltaTime;
        }

        // use time remaining after v = 0
        radiansToGoal -= 0.5f * inputVelocity * t0; //will be negative
        return Approach(0.0f, maxVelocity, radiansToGoal, maxAcceleration, deltaTime - t0, noOvershoot);
    }

    //calculates force needed across a single local angular velocity axis to align with a target, optionally allow overshooting
    private float Approach(float inputVelocity, float maxVelocity, float radiansToGoal, float maxAcceleration, float deltaTime, bool noOvershoot) {
        float deltaRadiansToGoal;		// amount rotated during time delta_t
        float effectiveAngularAcceleration;

        if (maxAcceleration == 0) {
            return inputVelocity;
        }

        if (noOvershoot && (inputVelocity * inputVelocity > 2.0f * 1.05f * maxAcceleration * radiansToGoal)) {
            inputVelocity = Mathf.Sqrt(2.0f * maxAcceleration * radiansToGoal);
        }

        if (inputVelocity * inputVelocity > 2.0f * 1.05f * maxAcceleration * radiansToGoal) {		// overshoot condition
            effectiveAngularAcceleration = 1.05f * maxAcceleration;
            deltaRadiansToGoal = inputVelocity * deltaTime - 0.5f * effectiveAngularAcceleration * deltaTime * deltaTime;

            if (deltaRadiansToGoal > radiansToGoal) {	// pass goal during this frame
                float timeToGoal = (-inputVelocity + Mathf.Sqrt(inputVelocity * inputVelocity + 2.0f * effectiveAngularAcceleration * radiansToGoal)) / effectiveAngularAcceleration;
                // get time to theta_goal and away
                inputVelocity -= effectiveAngularAcceleration * timeToGoal;
                return -Away(-inputVelocity, maxVelocity, 0.0f, maxAcceleration, deltaTime - timeToGoal, noOvershoot);
            }
            else {
                if (deltaRadiansToGoal < 0) {
                    // pass goal and return this frame
                    return 0.0f;
                }
                else {
                    // do not pass goal this frame
                    return inputVelocity - effectiveAngularAcceleration * deltaTime;
                }
            }
        }
        else if (inputVelocity * inputVelocity < 2.0f * 0.95f * maxAcceleration * radiansToGoal) {	// undershoot condition
            // find peak angular velocity
            float peakVelocitySqr = Mathf.Sqrt(maxAcceleration * radiansToGoal + 0.5f * inputVelocity * inputVelocity);
            if (peakVelocitySqr > maxVelocity * maxVelocity) {
                float timeToMaxVelocity = (maxVelocity - inputVelocity) / maxAcceleration;
                if (timeToMaxVelocity < 0) {
                    // speed already too high
                    // TODO: consider possible ramp down to below w_max
                    float outputVelocity = inputVelocity - maxAcceleration * deltaTime;
                    if (outputVelocity < 0) {
                        outputVelocity = 0.0f;
                    }
                    return outputVelocity;
                }
                else if (timeToMaxVelocity > deltaTime) {
                    // does not reach w_max this frame
                    return inputVelocity + maxAcceleration * deltaTime;
                }
                else {
                    // reaches w_max this frame
                    // TODO: consider when to ramp down from w_max
                    return maxVelocity;
                }
            }
            else {	// wp < w_max
                if (peakVelocitySqr > (inputVelocity + maxAcceleration * deltaTime) * (inputVelocity + maxAcceleration * deltaTime)) {
                    // does not reach wp this frame
                    return inputVelocity + maxAcceleration * deltaTime;
                }
                else {
                    // reaches wp this frame
                    float wp = Mathf.Sqrt(peakVelocitySqr);
                    float timeToPeakVelocity = (wp - inputVelocity) / maxAcceleration;

                    // accel
                    float outputVelocity = wp;
                    // decel
                    float timeRemaining = deltaTime - timeToPeakVelocity;
                    outputVelocity -= maxAcceleration * timeRemaining;
                    if (outputVelocity < 0) { // reached goal
                        outputVelocity = 0.0f;
                    }
                    return outputVelocity;
                }
            }
        }
        else {														// on target
            // reach goal this frame
            if (inputVelocity - maxAcceleration * deltaTime < 0) {
                // reach goal this frame

                return 0f;
            }
            else {
                // move toward goal
                return inputVelocity - maxAcceleration * deltaTime;
            }
        }
    }

    protected float ComputeLocalAV(float targetAngle, float localAV, float maxVel, float acceleration) {
        if (targetAngle > 0) {
            if (localAV >= 0) {
                return Approach(localAV, maxVel, targetAngle, acceleration, Time.fixedDeltaTime, false);
            }
            else {
                return Away(localAV, maxVel, targetAngle, acceleration, Time.fixedDeltaTime, false);
            }
        }
        else if (targetAngle < 0) {
            if (localAV <= 0) {
                return -Approach(-localAV, maxVel, -targetAngle, acceleration, Time.fixedDeltaTime, false);
            }
            else {
                return -Away(-localAV, maxVel, -targetAngle, acceleration, Time.fixedDeltaTime, false);
            }
        }
        else {
            return 0;
        }
    }

}


////todo this doesnt belong here, spline following should be a behavior
////assumes we are aligned with the spline we want to follow already
//public void AutoPilotFollowSpline() {
//    CurvySpline spline = FlightControls.spline;
//    if (spline == null || !spline.IsInitialized) return;
//    //when following a spline, need to check for obstructions and avoid them
//    float roll = 0f;
//    float splineSpeed = MaxSpeed;
//    float tf = FlightControls.splineTF;
//    if (spline == null) return;

//    CurvySplineSegment next = spline.TFToSegment(currentTF).NextSegment;
//    if (next != null) {
//        roll = next.transform.eulerAngles.z;
//        InfamySplineSegment segment = next.gameObject.GetComponent<InfamySplineSegment>();
//        if (segment != null) {
//            controls.SetThrottle(segment.throttle);

//        } else {
//            controls.SetThrottle(1f);
//        }
//    }


//    controls.SetThrottle(1f);
//    float dist = (controls.destination - transform.position).magnitude;
//    float speed = controls.throttle * MaxSpeed;
//    if (dist > MaxSpeed) {
//        speed = MaxSpeed;
//        currentTF = spline.GetNearestPointTF(transform.position + (transform.forward * speed * 0.75f));
//    }
//    controls.destination = spline.MoveByFast(ref currentTF, ref dr, speed * Time.fixedDeltaTime, CurvyClamping.Clamp);
//    Vector3 goalDirection = (controls.destination - transform.position).normalized;
//    TurnTowardsDirection(goalDirection, roll);
//    AdjustThrottleByVelocity();
//}

//private void TurnTowardsDirection(Vector3 direction, float roll = 0f, float rollTheta = 50f) {
//    roll = Util.WrapAngle180(roll);

//    float turnRateModifier = 1f;
//    float frameTurnModifier = 1f;// 0.5f;
//    Vector3 localTarget = transform.InverseTransformDirection(direction);
//    float targetAngleYaw = Mathf.Atan2(localTarget.x, localTarget.z);
//    float targetAnglePitch = -Mathf.Atan2(localTarget.y, localTarget.z);

//    Vector3 localAV = transform.InverseTransformDirection(rigidbody.angularVelocity);
//    float FrameTurnRateRadians = TurnRate * Mathf.Deg2Rad * Time.fixedDeltaTime;

//    float desiredX = targetAnglePitch;
//    float desiredY = targetAngleYaw;
//    float desiredZ = -targetAngleYaw + GetRollAngle();//roll * Mathf.Deg2Rad + GetRollAngle();
//    desiredZ = Util.WrapRadianPI(desiredZ);//stops endless spinning 

//    float TurnRateRadians = TurnRate * Mathf.Deg2Rad;

//    if (Mathf.Abs(desiredX) >= TurnRateRadians * turnRateModifier) {
//        localAV.x += FrameTurnRateRadians * Mathf.Sign(desiredX - TurnRateRadians * turnRateModifier);
//        localAV.x = Mathf.Clamp(localAV.x, -TurnRateRadians, TurnRateRadians);
//    } else {
//        if (desiredX >= localAV.x) {
//            localAV.x = Mathf.Clamp(localAV.x + FrameTurnRateRadians * frameTurnModifier, localAV.x, desiredX);
//        } else {
//            localAV.x = Mathf.Clamp(localAV.x - FrameTurnRateRadians * frameTurnModifier, desiredX, localAV.x);
//        }
//    }
//    float yawMod = 1f;
//    float rollMod = 1f;
//    if (Mathf.Abs(desiredY) >= TurnRateRadians * turnRateModifier) {
//        localAV.y += FrameTurnRateRadians * Mathf.Sign(desiredY - TurnRateRadians * turnRateModifier) * yawMod;
//        localAV.y = Mathf.Clamp(localAV.y, -TurnRateRadians, TurnRateRadians);
//    } else {
//        if (desiredY >= localAV.y) {
//            localAV.y = Mathf.Clamp(localAV.y + (FrameTurnRateRadians * frameTurnModifier * yawMod), localAV.y, desiredY);
//        } else {
//            localAV.y = Mathf.Clamp(localAV.y - (FrameTurnRateRadians * frameTurnModifier * yawMod), desiredY, localAV.y);
//        }
//    }

//    if (Mathf.Abs(desiredZ) >= TurnRateRadians * turnRateModifier) {
//        localAV.z += FrameTurnRateRadians * Mathf.Sign(desiredZ - TurnRateRadians * turnRateModifier) * rollMod;
//        localAV.z = Mathf.Clamp(localAV.z, -TurnRateRadians, TurnRateRadians);
//    } else {
//        if (desiredZ >= localAV.z) {
//            localAV.z = Mathf.Clamp(localAV.z + FrameTurnRateRadians * frameTurnModifier, localAV.z, desiredZ);
//        } else {
//            localAV.z = Mathf.Clamp(localAV.z - FrameTurnRateRadians * frameTurnModifier, desiredZ, localAV.z);
//        }
//    }

//    rigidbody.angularVelocity = transform.TransformDirection(localAV);
//}