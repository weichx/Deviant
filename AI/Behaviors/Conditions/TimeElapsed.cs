using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class TimeElapsed : Conditional {

    public Pilot pilot;
    public float time;
    private Timer timer;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        timer = new Timer(time);
    }

    public override TaskStatus OnUpdate() {
        if (timer.Ready) {
            Debug.Log("ok, ready");
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}