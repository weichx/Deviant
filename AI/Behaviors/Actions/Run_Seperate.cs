using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Run_Seperate : Action {

    public Pilot pilot;
    public Vector3 point;
    private Timer timer;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        point = pilot.transform.position;
        timer = new Timer();
    }

    public override void OnStart() {
        timer.Reset(5f);
    }

    public override TaskStatus OnUpdate() {

        if(timer.Ready) {
            return TaskStatus.Success;
        }

        //how long have we been seperating?
        //how much distance do we need?
        //ok, find a point -- expanded into safe / unsafe later
        //approach point
        //reach point
            //how much distance? not enough? repeat

        Vector3 escapeDirection = (pilot.transform.position - pilot.target.position).normalized;

        pilot.OrientWithAvoidance(escapeDirection, pilot.engines.speed, pilot.engines.speed);
        pilot.flightControls.SetThrottle(1f);
        return TaskStatus.Running;
    }
}