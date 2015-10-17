using UnityEngine;
using System.Collections.Generic;

public class WeaponGroup : MonoBehaviour {
    public string groupId;
    public string weaponId = WeaponDatabase.DefaultWeapon;
    public List<Firepoint> firepoints;
    public WeaponSpawner spawner;
    private AbstractWeapon weapon;
    public WeaponFiringParameters firingParameters;
    private int currentFirepointIndex = 0;
    private IWeapon weaponRef;

    public void Start() {
        spawner = WeaponSpawner.Get(weaponId);
        weaponRef = spawner.referenceWeapon;
        Entity entity = GetComponentInParent<Entity>();
        firepoints = new List<Firepoint>();
        firingParameters = new WeaponFiringParameters(entity);
        currentFirepointIndex = 0;
        Firepoint[] childFirepoints = GetComponentsInChildren<Firepoint>();
        for (int i = 0; i < childFirepoints.Length; i++) {
            if (childFirepoints[i].weaponGroupId == groupId) {
                firepoints.Add(childFirepoints[i]);
            }
        }
    }

    public void AddFirepoint(Firepoint firepoint) {
        firepoints.Add(firepoint);
    }
    
    public bool Fire() {
        if (firepoints.Count == 0 || spawner == null || !spawner.CanFire(firingParameters)) return false;
        firingParameters.hardpointTransform = firepoints[currentFirepointIndex].transform;
        spawner.Spawn(firingParameters);
        currentFirepointIndex = (currentFirepointIndex + 1) % firepoints.Count;
        firingParameters.lastFireTime = Time.time;
        return true;
    }

    public IWeapon ReferenceWeapon {
        get { return weaponRef; }
    }

    public float Range {
        get { return weaponRef.Range; }
    }

    public float Speed {
        get { return weaponRef.Speed; }
    }

    public float AspectFOV {
        get { return 0f; }
    }

    public bool CanFire {
        get { return spawner.CanFire(firingParameters); }
    }

    public bool IsLinked {
        get { return false; }
    }

    public float Ammo {
        get { return 0f; }
    }

    public float NextFireTime {
        get { return 0f; }
    }

    public bool IsFiring {
        get { return false; }
    }

    public bool HasActiveWarheads {
        get { return false; }
    }

    public static string DefaultId = "default";
}