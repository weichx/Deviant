using UnityEngine;

public enum AspectType {
    Predictive, Chase
}

public class StandardAspectMissile : AbstractWeapon {
    public bool aspectSeeking;
    public AspectType aspectType;
    public float aspectLockTime;
    [Range(1, 360)]
    public float aspectFOV;
    public float aspectDelay;
    public Entity target;
    public float turnRate;
    public float lifetime;

    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
        this.spawner = spawner;
        this.firingParameters = firingParameters;
    }

    public override bool CanFire(WeaponFiringParameters firingParameters) {
        float lastFireTime = firingParameters.lastFireTime;
        return Time.time - lastFireTime > fireRate;
    }
}