using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class OrientToTarget : Action {

    public Pilot pilot;
    public SensorSystem sensors;
    public WeaponSystem weapons;

    private Vector3 lastTargetPos;

    public override void OnAwake() {
        pilot = GetComponent<Pilot>();
        sensors = GetComponent<SensorSystem>();
        weapons = GetComponent<WeaponSystem>();
    }

    public static Vector3 Predict(Vector3 sPos, Vector3 tPos, Vector3 tLastPos, float pSpeed) {
        // Target velocity
        Vector3 tVel = (tPos - tLastPos) / Time.deltaTime;

        // Time to reach the target
        float flyTime = GetProjFlightTime(tPos - sPos, tVel, pSpeed);

        if (flyTime > 0)
            return tPos + flyTime * tVel;
        else
            return tPos;
    }

    static float GetProjFlightTime(Vector3 dist, Vector3 tVel, float pSpeed) {
        float a = Vector3.Dot(tVel, tVel) - pSpeed * pSpeed;
        float b = 2.0f * Vector3.Dot(tVel, dist);
        float c = Vector3.Dot(dist, dist);

        float det = b * b - 4 * a * c;

        if (det > 0)
            return 2 * c / (Mathf.Sqrt(det) - b);
        else
            return -1;
    }
    
    public override TaskStatus OnUpdate() {
        ////right now its really hard to predict position for something turning a lot, this algorithm works great for 
        ////when target is straight but fails consistently when target is chaning direction. Try to find a way to take
        ////into account the velocity direction trend of the target and integrate that into this algorithm
        //Entity target = sensors.Target;
        //Vector3 targetVelocity = sensors.Target.GetComponent<Rigidbody>().velocity;
        ////		vm_vec_scale_add(predicted_enemy_pos, enemy_pos, &target_moving_direction, aip->time_enemy_in_range * dist/weapon_speed);
        //float t = Vector3.Distance(transform.position, target.transform.position) / 100f; //100 is weapon speed
        //Vector3 predictedPosition = target.transform.position + targetVelocity * t;
        //Vector3 targetPosition = sensors.TargetPosition;
        //Vector3 toPredicted = (predictedPosition - pilot.transform.position).normalized;
        //pilot.flightControls.destination = predictedPosition;// +(toPredicted);// * 0.25f);
        ////Debug.DrawLine(pilot.transform.position, predictedPosition, Color.yellow, Time.deltaTime);
        //weapons.weaponGroups[0].Fire();
        //pilot.flightControls.throttle = 1f;
        //return TaskStatus.Running;

        pilot.flightControls.SetThrottle(1f);
        pilot.engines.OrientToDirection((pilot.target.position - transform.position).normalized);
        return TaskStatus.Success;
    }
}