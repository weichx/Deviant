using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Fire : Action {

    public Pilot pilot;
    public WeaponSystem weapons;
    public SensorSystem sensors;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        weapons = pilot.weaponSystem;
    }

    public override TaskStatus OnUpdate() {
        //todo replace with distance from weapon firepoint to target
        Vector3 toTarget = pilot.target.position - transform.position;
        float goalDot = Vector3.Dot(toTarget.normalized, transform.forward);

        //todo get weapon range
        float weaponRange = pilot.ActiveWeaponRange;
        if(toTarget.sqrMagnitude <= weaponRange * weaponRange && goalDot >= 0.98f) {
            if(pilot.Fire()) {
                return TaskStatus.Success;
            }            
            else {
                return TaskStatus.Failure;
            }
        }
        return TaskStatus.Failure;
    }
}