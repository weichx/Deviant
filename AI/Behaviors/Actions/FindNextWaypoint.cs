using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class FindNextWaypoint : Action {

    private Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        pilot.FindNextWaypoint();
        return TaskStatus.Success;
    }
}