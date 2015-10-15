using UnityEngine;

public class Beam : AbstractWeapon {
    public float beamScale = 1f;
    public float uvCycleTime;
    public float wanderRange = 2f;
    public float lifeTime;
    public LayerMask impactLayer;

    private LineRenderer lineRenderer;
    private Material beamMaterial;
    private float elapsedFireTime;
    private float beamLength;
    private bool hitLastFrame = false;
    private GameObject impact;
    protected Transform hardpoint;

    public void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        beamMaterial = lineRenderer.material;
    }

    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
        firingParameters.firingBeam = true;
        this.firingParameters = firingParameters;
        this.spawner = spawner;
        this.hardpoint = firingParameters.hardpointTransform;
        elapsedFireTime = 0f;
        beamLength = 0f;
        lineRenderer.SetPosition(1, new Vector3(0f, 0f, 0f));
        if (wanderRange != 0) {
            float accuracy = ((wanderRange + (wanderRange * (1 - firingParameters.accuracy))) / wanderRange) * wanderRange;
            transform.rotation *= Quaternion.Euler(UnityEngine.Random.insideUnitSphere * accuracy);
        }
    }

    public void Update() {
        elapsedFireTime += Time.deltaTime;
        if (elapsedFireTime >= lifeTime) {
            spawner.Despawn(gameObject);
            firingParameters.firingBeam = false;
            firingParameters.lastFireTime = Time.time;
            beamLength = 0f;
            lineRenderer.SetPosition(1, new Vector3(0f, 0f, 0f));
            hitLastFrame = false;
            if (impact) {
                ControlledDespawn despawner = impact.GetComponent<ControlledDespawn>();
                despawner.shouldDespawn = true;
                impact = null;
            }
            return;
        }
        transform.position = hardpoint.position;

        RaycastHit hit;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, beamLength, impactLayer)) {
            beamLength = (hit.point - transform.position).magnitude;
            if (!hitLastFrame) {
                hitLastFrame = true;
                impact = spawner.SpawnImpact(hit.point + hit.normal * 0.2f, Quaternion.identity);
            }
        } else {
            if (hitLastFrame && impact != null) {
                ControlledDespawn despawner = impact.GetComponent<ControlledDespawn>();
                despawner.shouldDespawn = true;
            }
            impact = null;
            hitLastFrame = false;
            beamLength = Mathf.Clamp(beamLength + (speed * Time.deltaTime), 0, range);
        }

        beamMaterial.SetTextureOffset("_MainTex", new Vector2(Time.time * uvCycleTime, 0f));
        lineRenderer.SetPosition(1, new Vector3(0f, 0f, beamLength));
        beamMaterial.SetTextureScale("_MainTex", new Vector2(1f, 1f));
    }

    public override bool CanFire(WeaponFiringParameters fp) {
        if (fp.firingBeam) return false;
        float lastFireTime = fp.lastFireTime;
        return Time.time - lastFireTime > fireRate;
    }
}