//using UnityEngine;

//public partial class Entity : MonoBehaviour {

//    [Header("Intersection Module")]
//    public OrientedBoundingBox orientedBoundingBox;
//    public Mesh quickCastMesh;

//    private QuickCastTriangle[] quickCastTriangles;

//    protected void IntersectionModule_Intialize() {
//        Collider collider = GetComponentInChildren<Collider>();
//        orientedBoundingBox = new OrientedBoundingBox(transform, collider);
//        GenerateQuickCastTriangles();
//    }

//    protected void IntersectionModule_Update() {
//        orientedBoundingBox.Update(transform.position, transform.rotation);     
//    }

//    //protected void IntersectionModule_DrawGizmos() {
//    //    if (orientedBoundingBox != null && drawOrientedBounds) {
//    //        orientedBoundingBox.Update(transform.position, transform.rotation);
//    //        orientedBoundingBox.DrawAsGizmo(Color.blue);
//    //    }
//    //    if (quickCastTriangles == null || !drawQuickCastMesh) return;
//    //    Quaternion rotation = transform.rotation;
//    //    Vector3 position = transform.position;
//    //    for (int i = 0; i < quickCastTriangles.Length; i++) {
//    //        QuickCastTriangle t = quickCastTriangles[i];
//    //        Vector3 point0 = (rotation * t.point0) + position;
//    //        Vector3 point1 = (rotation * t.point1) + position;
//    //        Vector3 point2 = (rotation * t.point2) + position;
//    //        Debug.DrawLine(point0, point1);
//    //        Debug.DrawLine(point1, point2);
//    //        Debug.DrawLine(point2, point0);
//    //    }
//    //}

//    public Vector3? GetRayIntersectionPoint(Ray ray) {
//        if (quickCastTriangles != null) {
//            float min = float.MaxValue;
//            Vector3? nearest = null;
//            Quaternion rotation = transform.rotation;
//            Vector3 position = transform.position;
//            for (int i = 0; i < quickCastTriangles.Length; i++) {
//                QuickCastTriangle t = quickCastTriangles[i];
//                Vector3 point0 = (rotation * t.point0) + position;
//                Vector3 point1 = (rotation * t.point1) + position;
//                Vector3 point2 = (rotation * t.point2) + position;
//                Vector3 normal = (rotation * t.normal) + position;
//                Vector3? intersection = Intersections.RayTriangleIntersection(ray, normal, point0, point1, point2);
//                if (intersection != null) {
//                    float sqrMag = ((Vector3)intersection - ray.origin).sqrMagnitude;
//                    if (sqrMag < min) {
//                        min = sqrMag;
//                        nearest = intersection;
//                    }
//                }
//            }
//            return nearest;
//        } else {
//            return orientedBoundingBox.GetRayIntersectionPoint(transform, ray);
//        }
//    }

//    public Vector3? GetRayIntersectionPoint(Vector3 origin, Vector3 direction) {
//        return GetRayIntersectionPoint(new Ray(origin, direction));
//    }

//    private void GenerateQuickCastTriangles() {
//        if (quickCastMesh == null) return;
//        int[] triangles = quickCastMesh.triangles;
//        Vector3[] vertices = quickCastMesh.vertices;

//        quickCastTriangles = new QuickCastTriangle[triangles.Length / 3];
//        float localScaleX = transform.localScale.x;
//        for (int i = 0; i < quickCastTriangles.Length; i++) {
//            QuickCastTriangle t = new QuickCastTriangle();
//            t.point0 = vertices[triangles[(i * 3) + 0]] * localScaleX;
//            t.point1 = vertices[triangles[(i * 3) + 1]] * localScaleX;
//            t.point2 = vertices[triangles[(i * 3) + 2]] * localScaleX;
//            t.normal = Vector3.Cross(t.point1 - t.point0, t.point2 - t.point0);
//            quickCastTriangles[i] = t;
//        }
//    }
//}