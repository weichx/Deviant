using System;
using UnityEngine;

public abstract class StandardProjectileWeapon : AbstractWeapon {
    public float raycastDistance = 3f;
    public float spreadFactor = 1f;
    public LayerMask impactLayer;
    protected float distanceTravelled = 0f;
    protected Vector3 forward;

    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
        this.spawner = spawner;
        this.firingParameters = firingParameters;
        this.distanceTravelled = 0f;
        transform.position = firingParameters.hardpointTransform.position;
        transform.rotation = firingParameters.hardpointTransform.rotation;
        if (spreadFactor != 0) {
            float accuracy = ((spreadFactor + (spreadFactor * (1 - firingParameters.accuracy))) / spreadFactor) * spreadFactor;
            transform.rotation *= Quaternion.Euler(UnityEngine.Random.insideUnitSphere * accuracy);
        }
        forward = transform.forward;
        spawner.SpawnMuzzleFlash(firingParameters.hardpointTransform);
        transform.position = transform.position + (forward * spawnOffset);
    }

    public void Update() {
        float speedStep = speed * Time.deltaTime;
        distanceTravelled += speedStep;
        transform.position += (forward * speedStep);
        RaycastHit hit;
        //distance travel check to avoid hitting our self, should work most of the time
        if (distanceTravelled > 1f && Physics.Raycast(new Ray(transform.position - forward * 2f, forward), out hit, raycastDistance, impactLayer)) {
            Entity victim = hit.transform.GetComponentInParent<Entity>();
            victim.ApplyDamage(firingParameters.entity, 10f);
            spawner.SpawnImpact(hit.point + hit.normal * 0.2f, Quaternion.identity, hit.transform);
            spawner.Despawn(gameObject);
        } else if (distanceTravelled >= range) {
            spawner.Despawn(gameObject);
        }
    }

    public override bool CanFire(WeaponFiringParameters firingParameters) {
        float lastFireTime = firingParameters.lastFireTime;
        return Time.time - lastFireTime > fireRate;
    }
}
