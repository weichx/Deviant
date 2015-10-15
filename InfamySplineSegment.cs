//using UnityEngine;
//using FluffyUnderware.Curvy.Utils;

//[RequireComponent(typeof(CurvySplineSegment))]
//public class InfamySplineSegment : MonoBehaviour {
//    public bool arrive = false;
//    public float waitTime = -1f;

//    public float throttle = 1f;
//    public float maxAcceleration = 1f;
//    public float maxTurnRate = -1f;
//    public float maxSpeed = -1f;

//    public void OnDrawGizmos() {
//        Gizmos.color = Color.green;
//        Gizmos.DrawSphere(transform.position, CurvyUtility.GetHandleSize(transform.position) * 1f * CurvySpline.GizmoControlPointSize);
//    }
//}