using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InFormation : Conditional {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override void OnStart() {

    }

    public override TaskStatus OnUpdate() {
        return pilot.InFormation ? TaskStatus.Success : TaskStatus.Failure;
    }
}