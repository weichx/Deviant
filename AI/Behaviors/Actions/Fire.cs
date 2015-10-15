using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Fire : Action {

    public Pilot pilot;
    public WeaponSystem weapons;
    public SensorSystem sensors;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        weapons = GetComponent<WeaponSystem>();
    }

    public override TaskStatus OnUpdate() {
        //todo replace with distance from weapon firepoint to target
        Vector3 toTarget = pilot.target.position - transform.position;
        float goalDot = Vector3.Dot(toTarget.normalized, transform.forward);

        //todo get weapon range
        if(toTarget.sqrMagnitude <= 2000 * 2000f && goalDot >= 0.98f) {
            if (weapons.weaponGroups[0].Fire()) {
                return TaskStatus.Success;
            }
            else {
                return TaskStatus.Failure;
            }
        }
        return TaskStatus.Failure;
    }
}