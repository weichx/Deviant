using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FlyTowards : Action {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        FlightControls flightControls = pilot.flightControls;
        AIEngineSystem engines = pilot.engines;
        float detectionRange = pilot.entity.radius + (engines.MaxSpeed * 2f);
        flightControls.destination = pilot.location.position;
        Vector3 goalDirection = (flightControls.destination - transform.position).normalized;
        Vector3 adjustedDirection = pilot.AdjustDirectionForAvoidance(goalDirection, detectionRange, detectionRange);

        pilot.flightControls.SetThrottle(1f);
        engines.OrientToDirection(adjustedDirection);

        return TaskStatus.Running;
    }
}