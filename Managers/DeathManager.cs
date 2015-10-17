using UnityEngine;
using System.Collections.Generic;

public class DeathManager : MonoBehaviour {
    //delay death until 5 frames in the future
    public float deathDelay = 5 * (0.16f);
    public Transform[] explosions;
    private Queue<DeathData> toDestroy;

    private static DeathManager instance;

    public void Awake() {
        instance = this;
        toDestroy = new Queue<DeathData>();
    }

    public static DeathManager Instance {
        get { return instance; }
    }

    public void LateUpdate() {
        float now = TimeManager.Timestamp;
        while(toDestroy.Count > 0) {
            DeathData data = toDestroy.Peek();
            if (now >= data.timestamp + deathDelay) {
                toDestroy.Dequeue();
                Destroy(data.entity.gameObject);
            }
            else {
                break;
            }
        }
    }

    public void Destroy(Entity entity) {
        //todo pool explosions
        //todo have pilot enter death state -- do real destroy after that
        EventManager.Instance.TriggerEvent(new Event_EntityDespawned(entity, TimeManager.Timestamp));
        entity.gameObject.SetActive(false);
        Instantiate(explosions[0], entity.transform.position, entity.transform.rotation);
        toDestroy.Enqueue(new DeathData(entity, TimeManager.Timestamp));
    }

    private struct DeathData {
        public float timestamp;
        public Entity entity;

        public DeathData(Entity entity, float timestamp) {
            this.entity = entity;
            this.timestamp = timestamp;
        }
    }
}