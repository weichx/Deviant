using UnityEngine;

public abstract class AbstractWeaponSpawnable : MonoBehaviour {

    [HideInInspector]
    public float spawnTime;
    [HideInInspector]
    public int poolIndex;
    [HideInInspector]
    public WeaponSpawner spawner;
    [HideInInspector]
    public WeaponSpawnerType type;

    public virtual void Spawn(WeaponSpawner spawner, WeaponSpawnerType type, int poolIndex, Vector3 position, Quaternion rotation) {
        spawnTime = Time.time;
        this.poolIndex = poolIndex;
        this.spawner = spawner;
        this.type = type;
        transform.position = position;
        transform.rotation = rotation;
    }

    public virtual void Spawn(WeaponSpawner spawner, WeaponSpawnerType type, int poolIndex, Transform parent) {
        spawnTime = Time.time;
        this.poolIndex = poolIndex;
        this.spawner = spawner;
        this.type = type;
        transform.parent = parent;
        transform.position = parent.position;
        transform.rotation = parent.rotation;
    }

}