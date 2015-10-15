using UnityEngine;

public struct PossibleCollision {
    public float radius;
    public float timeToImpact;
    public Vector3 velocity;
    public Transform transform;
    public float strengthModifier;
    public float horizon;

    public PossibleCollision(float radius, float timeToImpact, Vector3 velocity, Transform transform) {
        this.radius = radius;
        this.timeToImpact = timeToImpact;
        this.velocity = velocity;
        this.transform = transform;
        this.strengthModifier = 1f;
        this.horizon = 1f;
    }
}