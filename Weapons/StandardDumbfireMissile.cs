using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class StandardDumbfireMissile : AbstractWeapon {
    public float spreadFactor = 3f;
    protected Vector3 origin;
    protected new Rigidbody rigidbody;
    protected new Collider collider;
    protected float elapsedTime;

    public void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
        this.spawner = spawner;
        this.firingParameters = firingParameters;
        spawner.SpawnMuzzleFlash(firingParameters.hardpointTransform);
        transform.position = transform.position + (transform.forward * spawnOffset);
        float accuracy = ((spreadFactor + (spreadFactor * (1 - firingParameters.accuracy))) / spreadFactor) * spreadFactor;
        transform.rotation *= Quaternion.Euler(UnityEngine.Random.insideUnitSphere * accuracy);
        rigidbody.velocity = transform.forward * speed;
        rigidbody.angularVelocity = Vector3.zero;
        origin = transform.position;
        collider.enabled = false;
        elapsedTime = 0f;
    }

    public void Update() {
        elapsedTime += Time.deltaTime;
        if (!collider.enabled && elapsedTime >= 0.15f) {
            collider.enabled = true;
        }
        if ((origin - transform.position).sqrMagnitude >= range * range) {
            spawner.Despawn(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision) {
        spawner.SpawnImpact(transform.position, Quaternion.identity);
        spawner.Despawn(gameObject);
    }

    public override bool CanFire(WeaponFiringParameters firingParameters) {
        float lastFireTime = firingParameters.lastFireTime;
        return Time.time - lastFireTime > fireRate;
    }

}
