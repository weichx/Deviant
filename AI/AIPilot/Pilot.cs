using UnityEngine;
using System.Collections.Generic;

public class Pilot : MonoBehaviour {
    //todo consider building in drift for straight flights like formation node uses, might make things
    //look a bit more natural
    [HideInInspector]
    public Entity entity;
    [HideInInspector]
    public AIEngineSystem engines;
    [HideInInspector]
    public FlightControls flightControls;
    [HideInInspector]
    public new Rigidbody rigidbody;

    public Transform target;

    public int waypointIndex;//todo remove
    public WaypointCircuit circuit;//todo remove

    public Transform location;
    public Formation formation;

    public bool FormationLeader {
        get { return formation != null && formation.Leader == this;  }
    }

    public bool InFormation {
        get { return formation != null; }
    }

    public FormationNode FormationNode {
        get {
            if (formation == null) return null;
            return formation.GetNode(this);
        }
    }

    public void Start() {
        entity = GetComponent<Entity>();
        engines = GetComponentInChildren<AIEngineSystem>();
        flightControls = new FlightControls(transform.position);
        engines.SetFlightControls(flightControls);
        rigidbody = GetComponent<Rigidbody>();
        //todo remove this, replace it with orders / tasks
        waypointIndex = -1;
    }

    public float lastDistToTarget;
    public float lastDotToTarget;
    public float lastDotFromTarget;
    public float lastTargetSpeed;
    public float targetAquiredTimestamp;
    public float lastAttackEndTimestamp;
    public Vector3 lastAimedAtPosition;
    public Vector3 lastAimedAtVelocity;
    public Vector3 lastPredictedPosition;
    public float lastDamagedTimestamp;
    public Entity lastAttacker;
    public float timeEnemyInRange;
    public float timeSinceEnemyInRange;
    public float lastAttackTimestamp;
    public float lastSuccessfulShotTimestamp;

    public void Update() {
        
        //maybe update trends 4 times a frame instead of every frame
        //maybe only when situation recognizer runs
        //inside enemy turn: dotToTarget > dot from target && dot from Target > previousDotFromTarget
        //enemy turning away: dotFromTarget < previousDotFromTarget
        //trending closer to alignment: dot to target > previous dot to target
        //trending farther from alignment: dot to target < previous dot to target
        
    }

    public void OrientWithAvoidance(Vector3 direction, float detectionRange, float collisionHorizon) {
        Vector3 adjustedDirection = AdjustDirectionForAvoidance(direction, detectionRange, collisionHorizon);
        engines.OrientToDirection(adjustedDirection);
    }

    public float FindMaxArrivalSpeed(Vector3 goalDirection, Vector3 goalPosition) {
        float angle = Vector3.Angle(transform.forward, goalDirection) * Mathf.Deg2Rad;
        float distance = Vector3.Distance(transform.position, goalPosition);
        float turnRateRadians = engines.turnRate * Mathf.Deg2Rad;
        float speed = turnRateRadians * (distance / 2f) / Mathf.Cos(angle);
        speed += speed * engines.accelerationRate;
        if (speed < 0) speed = -speed;
        return speed;
    }

    public bool ReachableAtSpeed(Vector3 target, float speed, float turnRateRadians) {
        float radius = (speed / turnRateRadians) * 2f;// 1.5f;
        Vector3 toTarget = target - transform.position;
        Vector3 projectedToTarget = Vector3.ProjectOnPlane(toTarget, transform.forward).normalized * radius;
        Vector3 sphereCenter = transform.position + projectedToTarget;
        return (sphereCenter - target).sqrMagnitude >= radius * radius;
    }

    public void FindNextWaypoint() {
        waypointIndex = (waypointIndex + 1) % circuit.Waypoints.Length;
        flightControls.destination = circuit.Waypoints[waypointIndex].position;
    }

    //todo this can be cached / debounced to a few times a second instead of every frame. this would mean PotentialCollisions would need to be cached / updated
    public List<PossibleCollision> QueryPossibleCollisions(float detectionRange, float collisionHorizon) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        List<PossibleCollision> possibleCollisions = new List<PossibleCollision>(colliders.Length);
        for (var i = 0; i < colliders.Length; i++) {
            Collider collider = colliders[i];
            Transform otherTransform = collider.transform;
            if (collider.transform.IsChildOf(transform)) continue;

            Vector3 otherVelocity = Vector3.zero;
            Vector3 otherPosition = otherTransform.position;
            Vector3 size = collider.bounds.size;
            float otherRadius = Mathf.Max(size.x, Mathf.Max(size.y, size.z));// * t.localScale.x;
            Rigidbody otherRb = collider.attachedRigidbody;

            if (otherRb) {
                otherVelocity = otherRb.velocity;
            }

            float timeToCollision = TimeToCollision(otherPosition, otherVelocity, otherRadius);
            PossibleCollision pc = new PossibleCollision();
            pc.radius = otherRadius;
            pc.timeToImpact = timeToCollision;
            pc.transform = otherTransform;
            pc.velocity = otherVelocity;
            pc.strengthModifier = 1f;
            pc.horizon = collisionHorizon;
            if (timeToCollision < 0f || timeToCollision >= collisionHorizon) {
                continue;
            }
            else if(InFormation) {
                Pilot pilot = otherTransform.GetComponentInParent<Pilot>();
                if(pilot != null && pilot.formation != null) {
                    bool sameFormation = pilot.formation == formation;
                    if(sameFormation) { 
                        Vector3 toOther = (pilot.transform.position - transform.position).normalized;
                        bool isOtherIsBehind = Vector3.Dot(transform.forward, toOther) < 0;
                        if (FormationLeader && isOtherIsBehind) {
                            continue;
                        }
                        else if(pilot.FormationLeader && !isOtherIsBehind) {
                            continue;
                        }
                        else {
                            pc.strengthModifier = 0.1f;
                            pc.horizon = 0.001f;
                        }
                    }
                }
            }
            Debug.DrawLine(transform.position, otherTransform.position, Color.red);
            possibleCollisions.Add(pc);

        }
        return possibleCollisions;
    }

    public Vector3 AdjustDirectionForAvoidance(Vector3 direction, float detectionRange, float collisionHorizon) {
        List<PossibleCollision> possibleCollisions = QueryPossibleCollisions(detectionRange, collisionHorizon);
        return GetCollisionAvoidanceForce(possibleCollisions, direction);
    }

    protected Vector3 GetCollisionAvoidanceForce(List<PossibleCollision> possibleCollisions, Vector3 direction) {
        Vector3 force = direction * engines.speed;
        Vector3 velocity = rigidbody.velocity;
        Vector3 position = transform.position;

        for (var i = 0; i < possibleCollisions.Count; i++) {
            PossibleCollision pc = possibleCollisions[i];
            Vector3 avoidForce = Vector3.zero;
            float timeToImpact = pc.timeToImpact;

            if (timeToImpact == 0) {
                avoidForce += (position - pc.transform.position).normalized * engines.MaxSpeed * pc.strengthModifier;
            }
            else {
                avoidForce = position + velocity * timeToImpact - pc.transform.position - pc.velocity * timeToImpact;
                avoidForce.Normalize();

                float mag = 0f;
                if (timeToImpact >= 0 && timeToImpact <= pc.horizon) {
                    mag = (pc.horizon - timeToImpact) / (timeToImpact + 0.001f) * pc.strengthModifier;
                }
                avoidForce *= mag;
            }

            force += avoidForce;
        }
        //I think this wants to be a normalized weighted average so that the pilot can decide speed
        force /= (possibleCollisions.Count + 1);
        return force.normalized;
        //return (force.magnitude > MaxSpeed) ? force.normalized * MaxSpeed : force;
    }

    public float TimeToCollision(Vector3 otherPosition, Vector3 otherVelocity, float otherRadius) {
        float r = entity.radius + (otherRadius * 1.25f);
        Vector3 w = otherPosition - transform.position;
        float c = Vector3.Dot(w, w) - r * r;
        if (c < 0) {
            return 0;
        }
        Vector3 v = rigidbody.velocity - otherVelocity;
        float a = Vector3.Dot(v, v);
        float b = Vector3.Dot(w, v);
        float discr = b * b - a * c;
        if (discr <= 0)
            return -1;
        float tau = (b - Mathf.Sqrt(discr)) / a;
        if (tau < 0)
            return -1;
        return tau;
    }
}

