using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FlyInFormation : Action {

    public Pilot pilot;
    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        FormationNode node = pilot.FormationNode;
        FlightControls flightControls = pilot.flightControls;
        AIEngineSystem engines = pilot.engines;
        
        flightControls.destination = node.transform.position + (node.transform.forward * pilot.formation.LeaderSpeed);

        float detectionRange = pilot.entity.radius + (engines.speed * 2f);

        Vector3 goalDirection = (flightControls.destination - transform.position).normalized;
        Vector3 adjustedDirection = pilot.AdjustDirectionForAvoidance(goalDirection, detectionRange, detectionRange);

        float adjustedGoalDot = Vector3.Dot(goalDirection, adjustedDirection);
        float goalDot = Vector3.Dot(transform.forward, goalDirection);
        float speed = pilot.FindMaxArrivalSpeed(goalDirection, flightControls.destination);
        float throttle = speed / engines.MaxSpeed;

        ////adjust throttle based on alignment to target / pressure from collision avoidance
        if (adjustedGoalDot < 0 && throttle > 0.5f) {
            throttle = 0.5f;
        }
        else if (adjustedGoalDot < 0.33f && throttle > 0.6f) {
            throttle = 0.6f;
        }
        else if (adjustedGoalDot < 0.66f && throttle > 0.8f) {
            throttle = 0.8f;
        }
        else if (goalDot < 0.75f && throttle > 0.75f) {
            //moving more slowly reduces the arch of the turn, making it feel tighter
            throttle = 0.75f;
        }
        Debug.DrawLine(transform.position, flightControls.destination, Color.yellow);
        pilot.flightControls.SetThrottle(throttle);
        engines.OrientToDirection(adjustedDirection);
        return TaskStatus.Running;
    }
}