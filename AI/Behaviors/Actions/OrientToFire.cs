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
        //when target is straight but fails consistently when target is chaning direction. Try to find a way to take
        //into account the velocity direction trend of the target and integrate that into this algorithm
        Entity target = pilot.target.GetComponent<Entity>();
        Vector3 targetVelocity = target.GetComponent<Rigidbody>().velocity;
        //		vm_vec_scale_add(predicted_enemy_pos, enemy_pos, &target_moving_direction, aip->time_enemy_in_range * dist/weapon_speed);

        float t = Vector3.Distance(transform.position, target.transform.position) / 200f; //200 is weapon speed, todo: fix this
        Vector3 predictedPosition = target.transform.position + targetVelocity * t;

        Vector3 toPredicted = (predictedPosition - pilot.transform.position).normalized;

        pilot.flightControls.SetThrottle(1f);
        pilot.OrientWithAvoidance(toPredicted, 2 * pilot.engines.speed, 2 * pilot.engines.speed);
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