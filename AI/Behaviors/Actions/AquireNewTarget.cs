using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class AquireNewTarget : Action {

    public Pilot pilot;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
    }

    public override TaskStatus OnUpdate() {
        //todo expand this
        var hostiles = FactionManager.GetHostile(pilot.entity.factionId);
        if (hostiles.Count > 0) {
            pilot.target = hostiles[(int)Random.Range(0, hostiles.Count)].transform;
        }
        else {
            pilot.target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        return TaskStatus.Success;
    }
}