using UnityEngine;

public class RaptorMissile : StandardDumbfireMissile {}

//    public Transform target;
//    public float spreadFactor = 2f;
//    private float elapsedTime;
//    private new Rigidbody rigidbody;

//    public void Awake() {
//        base.Awake();
//        rigidbody = GetComponent<Rigidbody>();
//    }

//    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
//        this.spawner = spawner;
//        this.firingParameters = firingParameters;
//        this.elapsedTime = 0f;
//        float accuracy = ((spreadFactor + (spreadFactor * (1 - firingParameters.accuracy))) / spreadFactor) * spreadFactor;
//        transform.rotation *= Quaternion.Euler(UnityEngine.Random.insideUnitSphere * accuracy);
//        rigidbody.velocity = transform.forward * firingParameters.speed;
//        rigidbody.angularVelocity = Vector3.zero;
//    }

//    void Update() {
//        elapsedTime += Time.deltaTime;
//        if (elapsedTime >= firingParameters.lifetime) {
//            Despawn();
//        }
//    }

//    void OnCollisionEnter(Collision collision) {
//        spawner.SpawnImpact(transform.position, Quaternion.identity);
//        Despawn();
//    }

