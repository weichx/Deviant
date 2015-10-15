using UnityEngine;

public interface IWeapon {
    void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters);
    bool CanFire(WeaponFiringParameters firingParameters);
    float Speed { get; }
    float Range { get; }
    float FireRate { get; }
    string Name { get; }
    GameObject gameObject { get; }
    Transform transform { get; }
}