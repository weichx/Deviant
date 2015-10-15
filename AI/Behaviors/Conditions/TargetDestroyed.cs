using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TargetDestroyed : Conditional {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override void OnStart() {

    }

    public override TaskStatus OnUpdate() {
        return TaskStatus.Success;
    }
}