using UnityEngine;
using System.Collections.Generic;

public class Turret : MonoBehaviour {

    public Transform target; // todo this should be an entity or subsystem

    public bool locked;
    [HideInInspector]
    public bool multiPart;
    public string weaponId;
    public string turretGroupId;
    public float rotationSpeed = 45f;
    public float loftSpeed = 45f;
    [HideInInspector]
    public Transform barrel;
    [HideInInspector]
    public Vector3 turretNormal;
    [HideInInspector]
    public Vector3 barrelNormal;
    public float firingConeDegrees = -1f;
    public Firepoint[] firepoints;
    private float normalSign;

    private WeaponSpawner spawner;
    private WeaponFiringParameters firingParameters;
    private Entity entity;
    private int currentFirepointIndex;

    public void Awake() {
        currentFirepointIndex = 0;
        turretNormal = Vector3.Cross(turretNormal, transform.right);
        normalSign = (Vector3.Dot(turretNormal, transform.forward) < 0) ? 1 : -1;
        multiPart = barrel != null;
        if (!multiPart) {
            Vector3 toFirepoint = (firepoints[0].transform.position - transform.position).normalized;
            firepoints[0].transform.rotation = Quaternion.LookRotation(toFirepoint);
            if(firingConeDegrees < 0) {
                firingConeDegrees = 45f;
            }
        }
        else if(firingConeDegrees < 0) {
            firingConeDegrees = 176f;
        }
        //turret firepoints are created at import time
    }

    public void Start() {
        spawner = WeaponSpawner.Get(weaponId);
        entity = GetComponentInParent<Entity>();
        firingParameters = new WeaponFiringParameters(entity);
    }

    public void Update() {
        //todo implement control modes so ai can take direct control

        if (target == null || firingParameters == null || locked) return;
        Vector3 predictedPosition;
        Rigidbody rigid = target.GetComponent<Rigidbody>();
        if (rigid == null) {
            predictedPosition = target.position;
        } else {
            Vector3 targetVelocity = rigid.velocity;
            //todo if weapon type is aspect locking, this prediction isnt valid
            float t = Vector3.Distance(firepoints[currentFirepointIndex].transform.position, target.position) / spawner.referenceWeapon.Speed; 
            predictedPosition = target.position + targetVelocity * t;
        }

        AlignTo(predictedPosition);
        Fire(predictedPosition);
    }

    protected void AlignTo(Vector3 position) {
        if(!multiPart) return;
        Vector3 turretAxis = transform.rotation * turretNormal; //same as transform.TransformDirection(turretNormal)
        Vector3 localTarget = transform.InverseTransformPoint(position);
        //Vector3 projectedTarget = Vector3.ProjectOnPlane(localTarget, transform.up);

        float toAlignment = -normalSign * Mathf.Atan2(localTarget.x * normalSign, localTarget.y * normalSign) * Mathf.Rad2Deg;
        float turn = rotationSpeed * Time.deltaTime;
        float diff = Mathf.Clamp(toAlignment, -turn, turn);
        transform.Rotate(turretAxis, diff, Space.World);

        if (barrel == null) return;

        Vector3 barrelAxis = Vector3.Cross(barrel.up, barrel.rotation * barrelNormal);
        Vector3 barrelLocalTarget = barrel.InverseTransformPoint(position);
        //Vector3 barrelProjectedTarget = Vector3.ProjectOnPlane(barrelLocalTarget, barrelAxis);

        float toBarrelAlignment = -normalSign * Mathf.Atan2(barrelLocalTarget.y * normalSign, barrelLocalTarget.z * normalSign) * Mathf.Rad2Deg;
        float barrelTurn = loftSpeed * Time.deltaTime;
        float barrelDiff = Mathf.Clamp(toBarrelAlignment, -barrelTurn, barrelTurn);
        barrel.Rotate(barrelAxis, barrelDiff, Space.World);

        float fdot = Vector3.Dot(barrel.forward, transform.up);
        float udot = Vector3.Dot(barrel.forward, transform.forward);

        if (udot < 0) {
            barrel.localRotation = Quaternion.AngleAxis(90f, Vector3.left);
        } else if (fdot < 0) {
            barrel.localRotation = Quaternion.identity;
        }
    }

    //todo might need to special case some weapon types
    protected bool Fire(Vector3 predictedPosition) {
        if (firepoints.Length == 0 || spawner == null || !spawner.CanFire(firingParameters)) return false;
        Vector3 firepointPosition = firepoints[currentFirepointIndex].transform.position;

        if (multiPart) {
            Vector3 toTarget = predictedPosition - firepointPosition;
            RaycastHit hit;
            var result = Physics.Raycast(new Ray(firepointPosition, firepoints[currentFirepointIndex].transform.forward), out hit);
            if (result && hit.transform != target) {
                return false; //todo this should really only be done maybe twice a second and should account for friendlies, not just not target
            }
            if (Vector3.Dot(barrel.forward * normalSign, toTarget.normalized) > firingConeDegrees / 180f) {
                firingParameters.hardpointTransform = firepoints[currentFirepointIndex].transform;
                currentFirepointIndex = (currentFirepointIndex + 1) % firepoints.Length;
                spawner.Spawn(firingParameters);
                firingParameters.lastFireTime = Time.time;
            }
        } else {
            Vector3 toTarget = (target.position - firepointPosition).normalized;
            Vector3 toFirepoint = (firepointPosition - transform.position).normalized;          
            if (1 - Vector3.Dot(toTarget, toFirepoint) < firingConeDegrees / 180f) { 
                firingParameters.hardpointTransform = firepoints[currentFirepointIndex].transform;
                firingParameters.hardpointTransform.rotation = Quaternion.LookRotation(toTarget);
                currentFirepointIndex = (currentFirepointIndex + 1) % firepoints.Length;
                spawner.Spawn(firingParameters);
                firingParameters.lastFireTime = Time.time;
            }
        }
        return true;
    }

    public bool Inverted {
        get { return normalSign < 0; }
    }
}