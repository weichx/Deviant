using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class Area : MonoBehaviour {
    public float radius = 25f;
    public bool drawDebug = true;

    public void Awake() {
        Area.Register(name, this);
    }

    //todo -- checks should be done by looking up entity and comparing its position to center
    public void OnDrawGizmos() {
        if (drawDebug) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    public bool ContainsEntity(string id) {
        Entity entity = EntityManager.GetEntity(id);
        if (entity == null || !entity.isActiveAndEnabled) return false;
        return (radius * radius) >= (transform.position - entity.transform.position).sqrMagnitude;
    }

    public bool ContainsEntities(string[] entityIds) {
        float radiusSqr = radius * radius;
        Vector3 position = transform.position;
        Entity[] entities = EntityManager.GetEntities(entityIds);
        for (int i = 0; i < entities.Length; i++) {
            Entity ent = entities[i];
            if(ent == null || !ent.isActiveAndEnabled) return false;
            float distanceSqr = (position - ent.transform.position).sqrMagnitude;
            if (distanceSqr > radiusSqr) return false;
        }
        return true;
    }

    public int ContainedEntityCount() {
        //overlap sphere? debounced?
        return 0;
    }

    public Entity[] ContainedEntities() {
        //overlap sphere? debounced?
        return null;
    }

    public static Dictionary<string, Area> database;
    public static void Register(string id, Area area) {
        if (Area.database == null) Area.database = new Dictionary<string, Area>();
        if (!Area.database.ContainsKey(id)) {
            Area.database[id] = area;
        } else {
            throw new System.Exception("An area with id `" + id + "` is already registered");
        }
    }

}