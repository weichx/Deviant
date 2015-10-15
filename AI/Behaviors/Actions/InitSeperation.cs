using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class InitSeperation : Action {

    public Pilot pilot;
    public SharedFloat seperationStartTime;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override void OnStart() {
        seperationStartTime = TimeManager.Timestamp;
    }

    public override TaskStatus OnUpdate() {
        return TaskStatus.Success;
    }
}