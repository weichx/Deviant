using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DebugBreak : Action {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override void OnStart() {

    }

    public override TaskStatus OnUpdate() {
        Debug.Break();
        Debug.Log("Break");
        return TaskStatus.Success;
    }
}