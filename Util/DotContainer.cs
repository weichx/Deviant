using UnityEngine;

public struct DotContainer {
    public float right;
    public float up;
    public float forward;

    public DotContainer(float right = 0, float up = 0, float forward = 0) {
        this.right = right;
        this.up = up;
        this.forward = forward;
    }

    public override string ToString() {
        float f = forward;
        float r = right;
        float u = up;
        if (f < 0.0001f && f > -0.0001f) f = 0;
        if (r < 0.0001f && r > -0.0001f) r = 0;
        if (u < 0.0001f && u > -0.0001f) u = 0;
        return "Right: " + r + " Up: " + u + " Forward: " + f;
    }

    public void Clear() {
        this.right = 0;
        this.forward = 0;
        this.up = 0;
    }

    public static DotContainer ToVector(Transform origin, Vector3 target) {
        var toTarget = (target - origin.position).normalized;
        var right = Vector3.Dot(toTarget, origin.right);
        var up = Vector3.Dot(toTarget, origin.up);
        var forward = Vector3.Dot(toTarget, origin.forward);
        return new DotContainer(right, up, forward);
    }
}

//things to do --
//waypoints
//dog fight
//flesh out ai states
//flesh out ai orders
//entity data base + queries
//more weapons
//formations
//multiple ships
//debug output
//entity editor
//player flying
//componentize