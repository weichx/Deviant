using UnityEngine;
using System.Collections;

public class OrientedBoundingBox {
    public Vector3 center;
    public Vector3 extents;
    public Vector3[] points;
    private Vector3[] topQuad;
    private Vector3[] bottomQuad;
    private Vector3[] leftQuad;
    private Vector3[] rightQuad;
    private Vector3[] forwardQuad;
    private Vector3[] backwardQuad;
    public Transform transform;
    public Quaternion rotation;

    public OrientedBoundingBox(GameObject target, Collider collider) {
        var rotation = target.transform.rotation;
        target.transform.rotation = Quaternion.identity;
        var bounds = collider.bounds;
        target.transform.rotation = rotation;
        this.transform = target.transform;
        Construct(bounds.center, bounds.extents, rotation);
    }

    public OrientedBoundingBox(Transform target, Collider collider) {
        var rotation = target.rotation;
        target.rotation = Quaternion.identity;
        if (collider != null) {
            var bounds = collider.bounds;
            target.rotation = rotation;
            this.transform = target;
            Construct(bounds.center, bounds.extents, rotation);
        }
    }

    public OrientedBoundingBox(Vector3 center, Vector3 extents, Quaternion rotation) {
        Construct(center, extents, rotation);
    }

    private void Construct(Vector3 center, Vector3 extents, Quaternion rotation) {
        this.center = center;
        this.extents = extents;
        this.rotation = rotation;
        this.points = new Vector3[8];
        this.topQuad = new Vector3[4];
        this.bottomQuad = new Vector3[4];
        this.rightQuad = new Vector3[4];
        this.leftQuad = new Vector3[4];
        this.forwardQuad = new Vector3[4];
        this.backwardQuad = new Vector3[4];
        Update(center, extents, rotation);
    }

    public Vector3? GetRayIntersectionPoint(Transform transform, Ray ray) {
        Vector3?[] results = new Vector3?[6];

        results[0] = Intersections.RayQuadIntersection(ray, transform.up, topQuad);
        results[1] = Intersections.RayQuadIntersection(ray, transform.up, bottomQuad);
        results[2] = Intersections.RayQuadIntersection(ray, transform.right, leftQuad);
        results[3] = Intersections.RayQuadIntersection(ray, transform.right, rightQuad);
        results[4] = Intersections.RayQuadIntersection(ray, transform.forward, forwardQuad);
        results[5] = Intersections.RayQuadIntersection(ray, transform.forward, backwardQuad);

        Vector3? closest = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < results.Length; i++) {
            if (results[i] != null) {
                float sqrLength = (((Vector3)results[i]) - ray.origin).sqrMagnitude;
                if (sqrLength < minDistance) {
                    closest = results[i];
                    minDistance = sqrLength;
                }
            }
        }
        return closest;
    }

    public Vector3? GetRayIntersectionPoint(Transform transform, Vector3 origin, Vector3 direction) {
        return GetRayIntersectionPoint(transform, new Ray(origin, direction));
    }

    public bool RayIntersects(Transform transform, Ray ray) {
        return Intersections.RayQuadIntersection(ray, transform.up, topQuad) != null ||
               Intersections.RayQuadIntersection(ray, transform.right, rightQuad) != null ||
               Intersections.RayQuadIntersection(ray, transform.forward, forwardQuad) != null ||
               Intersections.RayQuadIntersection(ray, transform.up, bottomQuad) != null ||
               Intersections.RayQuadIntersection(ray, transform.right, leftQuad) != null ||
               Intersections.RayQuadIntersection(ray, transform.forward, backwardQuad) != null;
    }

    public Vector3? GetLineSegmentIntersectionPoint(Transform transform, Ray ray, float length) {
        Vector3?[] results = new Vector3?[6];

        results[0] = Intersections.RayQuadIntersection(ray, transform.up, topQuad);
        results[1] = Intersections.RayQuadIntersection(ray, transform.up, bottomQuad);
        results[2] = Intersections.RayQuadIntersection(ray, transform.right, leftQuad);
        results[3] = Intersections.RayQuadIntersection(ray, transform.right, rightQuad);
        results[4] = Intersections.RayQuadIntersection(ray, transform.forward, forwardQuad);
        results[5] = Intersections.RayQuadIntersection(ray, transform.forward, backwardQuad);

        Vector3? closest = null;
        float minDistance = float.MaxValue;
        float sqrLength = length * length;

        for (int i = 0; i < results.Length; i++) {
            if (results[i] != null) {
                float sqrDistance = (((Vector3)results[i]) - ray.origin).sqrMagnitude;
                if (sqrLength >= sqrDistance && sqrDistance < minDistance) {
                    closest = results[i];
                    minDistance = sqrLength;
                }
            }
        }
        return closest;
    }

    public Vector3 ClosestPoint(Vector3 point) {
        Vector3 d = point - center;
        Vector3 q = center;
        float upDist = Vector3.Dot(d, transform.up);
        if (upDist > extents.y) upDist = extents.y;
        if (upDist < -extents.y) upDist = -extents.y;
        q += upDist * transform.up;

        float rightDist = Vector3.Dot(d, transform.right);
        if (rightDist > extents.x) rightDist = extents.x;
        if (rightDist < -extents.y) rightDist = -extents.x;
        q += rightDist * transform.right;

        float forwardDist = Vector3.Dot(d, transform.forward);
        if (forwardDist > extents.z) forwardDist = extents.z;
        if (forwardDist < -extents.z) forwardDist = -extents.z;
        q += forwardDist * transform.forward;
        return q;
    }

    public void Update(Vector3 center) {
        Update(center, extents, rotation);
    }

    public void Update(Vector3 center, Quaternion rotation) {
        Update(center, extents, rotation);
    }

    public void Update(Quaternion rotation) {
        Update(center, extents, rotation);
    }

    public void Update(Vector3 center, Vector3 extents, Quaternion rotation) {
        this.center = center;
        this.extents = extents;
        this.rotation = rotation;

        var x = extents.x;
        var y = extents.y;
        var z = extents.z;

        points[0] = (rotation * new Vector3(-x, y, -z)) + center;  //top left backward
        points[1] = (rotation * new Vector3(-x, y, z)) + center;   //top left forward
        points[2] = (rotation * new Vector3(x, y, -z)) + center;   //top right backward
        points[3] = (rotation * new Vector3(x, y, z)) + center;    //top right forward
        points[4] = (rotation * new Vector3(-x, -y, -z)) + center; //bottom left backward
        points[5] = (rotation * new Vector3(-x, -y, z)) + center;  //bottom left forward
        points[6] = (rotation * new Vector3(x, -y, -z)) + center;  //bottom right backward
        points[7] = (rotation * new Vector3(x, -y, z)) + center;   //bottom right forward
        topQuad[0] = points[2];
        topQuad[1] = points[0];
        topQuad[2] = points[1];
        topQuad[3] = points[3];
        bottomQuad[0] = points[6];
        bottomQuad[1] = points[4];
        bottomQuad[2] = points[5];
        bottomQuad[3] = points[7];
        leftQuad[0] = points[0];
        leftQuad[1] = points[1];
        leftQuad[2] = points[5];
        leftQuad[3] = points[6];
        rightQuad[0] = points[2];
        rightQuad[1] = points[3];
        rightQuad[2] = points[7];
        rightQuad[3] = points[6];
        forwardQuad[0] = points[7];
        forwardQuad[1] = points[3];
        forwardQuad[2] = points[1];
        forwardQuad[3] = points[5];
        backwardQuad[0] = points[6];
        backwardQuad[1] = points[2];
        backwardQuad[2] = points[4];
        backwardQuad[3] = points[0];
    }

    public void DrawDebug(Color color) {
        Debug.DrawLine(points[0], points[1], color);
        Debug.DrawLine(points[2], points[3], color);
        Debug.DrawLine(points[4], points[5], color);
        Debug.DrawLine(points[6], points[7], color);

        Debug.DrawLine(points[0], points[2], color);
        Debug.DrawLine(points[2], points[6], color);
        Debug.DrawLine(points[6], points[4], color);
        Debug.DrawLine(points[4], points[0], color);

        Debug.DrawLine(points[1], points[3], color);
        Debug.DrawLine(points[3], points[7], color);
        Debug.DrawLine(points[7], points[5], color);
        Debug.DrawLine(points[5], points[1], color);
    }

    public void DrawAsGizmo(Color color) {
        Gizmos.color = color;
        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[4], points[5]);
        Gizmos.DrawLine(points[6], points[7]);

        Gizmos.DrawLine(points[0], points[2]);
        Gizmos.DrawLine(points[2], points[6]);
        Gizmos.DrawLine(points[6], points[4]);
        Gizmos.DrawLine(points[4], points[0]);

        Gizmos.DrawLine(points[1], points[3]);
        Gizmos.DrawLine(points[3], points[7]);
        Gizmos.DrawLine(points[7], points[5]);
        Gizmos.DrawLine(points[5], points[1]);
    }
}