using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class DeathManager : MonoBehaviour {
    public Transform[] explosions;
    public List<Entity> deadEntities;
    public List<Entity> toDestroy;

    private static DeathManager instance;

    public void Awake() {
        instance = this;
        deadEntities = new List<Entity>();
        toDestroy = new List<Entity>();
    }

    public static DeathManager Instance {
        get { return instance; }
    }

    public void LateUpdate() {
        for(int i = 0; i < toDestroy.Count; i++) {
            Destroy(toDestroy[i].gameObject);
        }
        toDestroy.Clear();
    }

    public void Destroy(Entity entity) {
        //todo pool explosions
        //todo have pilot enter death state -- do real destroy after that
        EventManager.Instance.TriggerEvent(new Event_EntityDespawned(entity, TimeManager.Timestamp));
        Instantiate(explosions[0], entity.transform.position, entity.transform.rotation);
        toDestroy.Add(entity);
    }
}