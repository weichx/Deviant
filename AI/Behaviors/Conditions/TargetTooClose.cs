using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetTooClose : Conditional {

    public Pilot pilot;
    public SensorSystem sensors;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        sensors = GetComponent<SensorSystem>();
    }

    public override TaskStatus OnUpdate() {
        Vector3 toTarget = pilot.target.position - transform.position;
        float distance = toTarget.sqrMagnitude;
        toTarget.Normalize();

        bool reachable = pilot.ReachableAtSpeed(pilot.target.position, pilot.engines.speed, pilot.engines.TurnRate_Radians);

        if (distance < (pilot.entity.radius * pilot.entity.radius) * 25f || !reachable) { //todo calculate this based off entity size
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}