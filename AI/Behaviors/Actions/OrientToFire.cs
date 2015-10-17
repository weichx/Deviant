using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class OrientToFire : Action {

    public Pilot pilot;
    public WeaponSystem weapons;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        weapons = GetComponent<WeaponSystem>();
    }

    public override TaskStatus OnUpdate() {
        //right now its really hard to predict position for something turning a lot, this algorithm works great for 
        //when target is straight but fails consistently when target is changing direction. Try to find a way to take
        //into account the velocity direction trend of the target and integrate that into this algorithm
        if (pilot.target == null) return TaskStatus.Failure;
        Entity target = pilot.target.GetComponent<Entity>();
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        float t = Vector3.Distance(transform.position, target.transform.position) / pilot.ActiveWeaponSpeed;
        Vector3 predictedPosition = target.transform.position + targetVelocity * t;

        Vector3 toPredicted = (predictedPosition - pilot.transform.position).normalized;

        pilot.flightControls.SetThrottle(1f);
        pilot.OrientWithAvoidance(toPredicted, 2f * pilot.engines.speed, 2f * pilot.engines.speed);
        return TaskStatus.Success;
    }
}

//attack_joust
//attack_get_behind
//attack_from_behind
//attack_normal
//pursuit curves
    //lead -- close on faster moving target
    //pure -- close on target, not as rapid as lead
    //lag -- let target get space from you, good for when in close / locking missiles



//evade_barrel_roll - better if already moving to one side

//coordinated maneuvers could be cool