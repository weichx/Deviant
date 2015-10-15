using BehaviorDesigner.Runtime.Tasks;

public class IsFormationLeader : Conditional {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        return pilot.FormationLeader ? TaskStatus.Success : TaskStatus.Failure;
    }
} 