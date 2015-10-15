using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class JoinOrCreateFormation : Action {

    public Pilot pilot;
    public Formation formation;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override void OnStart() {
        Formation formation = FormationManager.GetAvailableFormation();
        if(formation == null) {
            formation = FormationManager.CreateFormation();
            formation.SetLeader(pilot);
        }
        else {
            formation.AddMember(pilot);
        }
    }

    public override TaskStatus OnUpdate() {
        return TaskStatus.Success;
    }
}