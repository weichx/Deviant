using UnityEngine;
using System.Collections;

public class Intersections  {

    public static Vector3? RayPlaneIntersection(Ray ray, Vector3 planeNormal, Vector3 planePoint) {
        float dot = Vector3.Dot(planeNormal, ray.direction);
        if (dot == 0) return null;
        return ray.origin + ray.direction * (-Vector3.Dot(planeNormal, ray.origin - planePoint) / dot);
    }

    public static Vector3? RayTriangleIntersection(Ray ray, Vector3 normal, Vector3[] points) {
        Vector3? planeIntersection = RayPlaneIntersection(ray, normal, points[0]);
        if (planeIntersection == null) return null;
        if (Vector3.Dot(Vector3.Cross(points[1] - points[0], (Vector3)planeIntersection - points[0]), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(points[2] - points[1], (Vector3)planeIntersection - points[1]), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(points[0] - points[2], (Vector3)planeIntersection - points[2]), normal) < 0) return null;
        return planeIntersection;
    }

    public static Vector3? RayTriangleIntersection(Ray ray, Vector3 normal, Vector3 point0, Vector3 point1, Vector3 point2) {
        Vector3? planeIntersection = RayPlaneIntersection(ray, normal, point0);
        if (planeIntersection == null) return null;
        if (Vector3.Dot(Vector3.Cross(point1 - point0, (Vector3)planeIntersection - point0), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(point2 - point1, (Vector3)planeIntersection - point1), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(point0 - point2, (Vector3)planeIntersection - point2), normal) < 0) return null;
        return planeIntersection;
    }

   public static Vector3? RayQuadIntersection(Ray ray, Vector3 normal, Vector3[] points) {
        Vector3? planeIntersection = RayPlaneIntersection(ray, normal, points[0]);
        if (planeIntersection == null) return null;
        if (Vector3.Dot(Vector3.Cross(points[1] - points[0], (Vector3)planeIntersection - points[0]), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(points[2] - points[1], (Vector3)planeIntersection - points[1]), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(points[3] - points[2], (Vector3)planeIntersection - points[2]), normal) < 0) return null;
        if (Vector3.Dot(Vector3.Cross(points[0] - points[3], (Vector3)planeIntersection - points[3]), normal) < 0) return null;
        return planeIntersection;
    }

    public static Vector3? RayPolygonIntersection(Ray ray, Vector3 normal, Vector3[] points) {
        Vector3? planeIntersection = RayPlaneIntersection(ray, normal, points[0]);
        if (planeIntersection == null) return null;
        int n = points.Length;
        for (int i = 0; i < points.Length; i++) {
            if (Vector3.Dot(Vector3.Cross(points[(i + 1) % n] - points[i], (Vector3)planeIntersection - points[i]), normal) < 0) return null;
        }
        return planeIntersection;
    }

    public static Vector3? LineTriangleIntersection(Ray ray, float length, Vector3 triangleNormal, Vector3[] trianglePoints) {
        Vector3? result = RayTriangleIntersection(ray, triangleNormal, trianglePoints);
        if (result == null) return null;
        float sqrDistance = (ray.origin - (Vector3)result).sqrMagnitude;
        if (sqrDistance <= length * length) {
            return result;
        } else {
            return null;
        }
    }

    public static Vector3? LineQuadIntersection(Ray ray, float length, Vector3 quadNormal, Vector3[] quadPoints) {
        Vector3? result = RayQuadIntersection(ray, quadNormal, quadPoints);
        if (result == null) return null;
        float sqrDistance = (ray.origin - (Vector3)result).sqrMagnitude;
        if (sqrDistance <= length * length) {
            return result;
        } else {
            return null;
        }
    }

}
