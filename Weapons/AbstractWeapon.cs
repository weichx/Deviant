using System;
using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour, IWeapon {
    protected WeaponFiringParameters firingParameters;
    protected WeaponSpawner spawner;

    public float spawnOffset;
    public float speed;
    public float range;
    public float fireRate;

    public float Speed {
        get { return speed; }
    }

    public float Range {
        get { return range; }
    }

    public float FireRate {
        get { return fireRate; }
    }

    public abstract void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters);

    public abstract bool CanFire(WeaponFiringParameters firingParameters);

    public string Name {
        get { return GetType().ToString(); }
    }
}

//todo edit firepoints with Handles.PositionHandle http://docs.unity3d.com/ScriptReference/Handles.PositionHandle.html

//ai will need a highlevel weapon knowledge interface to see about getting weapon locks
//or beams etc. basics are Range, Speed, Agility(missile), AspectSeeking, AspectSeekingFOV?, AspectSeekingTime?
//probably needs a 'class' also to tell the difference between torpedoes and missiles etc.

//todo Accuracy might make the most sense as an AI property,
//maybe add AIPilot / AITurretSysem as a field on firing parameters
//or maybe read accuracy off of AIPilot / AITurretSystem and write to 
//firing parameters.accuracy

//maybe scale accuracy, damage, range others by difficulty level or AI Skill

//todo handle linking weapons by introducing a new set of 
//weapon groups that are tagged as linked. makes it easier than
//finding each weapon and seeing if it can be linked maybe

//add range, hull damage, shield damage, isAspectSeeking

//add IAspectSeekingWeapon
//AspectFOV
//AspectLockTime
