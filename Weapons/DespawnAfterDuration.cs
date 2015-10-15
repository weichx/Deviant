using UnityEngine;

public class DespawnAfterDuration : AbstractWeaponSpawnable {

    public float duration = 1f;

    public void Update() {
        if (Time.time - spawnTime > duration) {
            spawner.Despawn(this);
        }
    }
}