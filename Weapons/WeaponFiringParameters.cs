using System;
using System.Collections.Generic;
using UnityEngine;

//todo handle inter weapon group linking
//later todo, handle cross weapon group linking

public class WeaponFiringParameters {
    public Entity entity;

    public float lastFireTime;
    public float targetAquiredTime;
    public float totalAspectLockTime;
    public float currentAspectLockTime;
    public float longestAspectLockTime;

    public float aspectLockTime;
    public float accuracy;
    public bool firingBeam;

    //for turrets this is turret or barrel, for fixed weapons on fighters this is the root model object
    //firing points are relative (in local space) to this transform
    public Transform hardpointTransform; 


    public WeaponFiringParameters(Entity entity) {
        this.entity = entity;
        this.lastFireTime = 0f;
        this.targetAquiredTime = 0f;
        this.totalAspectLockTime = 0f;
        this.currentAspectLockTime = 0f;
        this.longestAspectLockTime = 0f;
        this.aspectLockTime = -1f;
        this.accuracy = 1f;
        this.firingBeam = false;//abstract with a beam id
    }

    public void UpdateAspectLockTime(IWeapon weapon) {
        //if (turret != null && turret.target != null) {

        //} else {
        //    SensorSystem sensors = entity.sensorSystem;
          //  if (!weapon.AspectSeeking || sensors == null) return;
            //if (sensors.TargetInAspectRange(weapon.AspectRange, weapon.AspectFOV)) {
            //    totalAspectLockTime += Time.deltaTime;
            //    currentAspectLockTime += Time.deltaTime;
            //    if (currentAspectLockTime > longestAspectLockTime) {
            //        longestAspectLockTime = currentAspectLockTime;
            //    }
            //} else {
            // currentAspecLockTime = 0f;
            //}
       // }
    }

    public void TargetAquired(Entity target) {
        this.totalAspectLockTime = 0f;
        this.currentAspectLockTime = 0f;
        this.longestAspectLockTime = 0f;
        this.targetAquiredTime = Time.time;
    }

}

