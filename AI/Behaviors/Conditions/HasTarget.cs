using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class HasTarget : Conditional {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        return pilot.target != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}