using UnityEngine;
using System.Collections.Generic;

public class NavPoint : MonoBehaviour {

    public float radius = 10f;
    public bool drawDebug = true;

    public void Awake() {
        NavPoint.Register(name, this);
    }

    public void OnDrawGizmos() {
        if (drawDebug) {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    public bool Reached(string entityId) {
        Entity entity = EntityManager.GetEntity(entityId);
        if(entity == null) return false;
        float distanceSquared = (transform.position - entity.transform.position).sqrMagnitude;
        return radius * radius >= distanceSquared;
    }

    public static Dictionary<string, NavPoint> database;
    public static void Register(string id, NavPoint area) {
        if (NavPoint.database == null) NavPoint.database = new Dictionary<string, NavPoint>();
        if (!NavPoint.database.ContainsKey(id)) {
            NavPoint.database[id] = area;
        } else {
            throw new System.Exception("A NavPoint with id `" + id + "` is already registered");
        }
    }
}