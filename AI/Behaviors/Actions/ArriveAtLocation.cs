using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class ArriveAtLocation : Action {

    public Pilot pilot;

    public override void OnStart() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        FlightControls flightControls = pilot.flightControls;
        AIEngineSystem engines = pilot.engines;

        float distanceSqr = (flightControls.destination - transform.position).sqrMagnitude;
        float entityRadius = pilot.entity.radius;

        if (distanceSqr < entityRadius * entityRadius && engines.speed <= 2f) {
            flightControls.SetThrottle(0f);
            flightControls.destination = transform.position;
            return TaskStatus.Success;
        }

        float stoppingDistance = engines.speed / (2 * engines.accelerationRate);
        float detectionRange = entityRadius + (engines.speed * 2f);
        bool onApproach = distanceSqr <= stoppingDistance * stoppingDistance;

        if (engines.Accelerating && !onApproach) {
            detectionRange = entityRadius + (engines.MaxSpeed * 2f);
        }
        else if (onApproach) {
            detectionRange = entityRadius + engines.speed;
        }

        Vector3 goalDirection = (flightControls.destination - transform.position).normalized;
        Vector3 adjustedDirection = pilot.AdjustDirectionForAvoidance(goalDirection, detectionRange, detectionRange);
        float adjustedGoalDot = Vector3.Dot(goalDirection, adjustedDirection);
        float goalDot = Vector3.Dot(transform.forward, goalDirection);

        if (onApproach) {
            pilot.flightControls.SetThrottle(0f);
        }
        else {
            float speed = pilot.FindMaxArrivalSpeed(goalDirection, flightControls.destination);
            float throttle = speed / engines.MaxSpeed;

            //adjust throttle based on alignment to target / pressure from collision avoidance
            if (adjustedGoalDot < 0 && throttle > 0.4f) {
                throttle = 0.4f;
            }
            else if (adjustedGoalDot < 0.33f && throttle > 0.6f) {
                throttle = 0.60f;
            }
            else if (adjustedGoalDot < 0.66f && throttle > 0.8f) {
                throttle = 0.8f;
            }
            else if (goalDot < 0.75f) {
                //moving more slowly reduces the arch of the turn, making it feel tighter
                throttle = 0.5f;
            }

            pilot.flightControls.SetThrottle(throttle);
        }

        engines.OrientToDirection(adjustedDirection);
        return TaskStatus.Running;
    }
}