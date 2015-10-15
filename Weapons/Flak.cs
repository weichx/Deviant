//#if UNITY_EDITOR
//using System;
//using UnityEngine;
//using UnityEditor;

//public class Flak : AbstractWeapon {

//    protected SerializedObject so;
//    protected ParticleSystem system;
//    protected ParticleCollisionEvent[] collisionEvents;
//    protected Transform hardpoint;

//	public void Awake () {
//        system = GetComponent<ParticleSystem>();
//        system.loop = false;
//        system.Clear(true);
//        collisionEvents = new ParticleCollisionEvent[(int)(system.maxParticles * 0.1f)];
//        so = new SerializedObject(system);
//        system.startSpeed = speed;
//        system.startLifetime = range / speed;
//	}

//    public override void Fire(WeaponSpawner spawner, WeaponFiringParameters firingParameters) {
//        this.spawner = spawner;
//        this.firingParameters = firingParameters;
//        this.hardpoint = firingParameters.hardpointTransform;
//        float spreadFactor = so.FindProperty("ShapeModule.angle").floatValue;
//        float accuracy = ((spreadFactor + (spreadFactor * (1 - firingParameters.accuracy))) / spreadFactor) * spreadFactor;
//        so.FindProperty("ShapeModule.angle").floatValue = accuracy;
//        so.ApplyModifiedProperties();
//        transform.position += transform.forward * spawnOffset;
//        system.Play(true);
//    }

//	public void Update () {
//        transform.position = hardpoint.position;
//        transform.rotation = hardpoint.rotation;
//        transform.position += transform.forward * spawnOffset;
//        if (!system.isPlaying) {
//            system.Clear();
//            hardpoint = null;
//            spawner.Despawn(gameObject);
//        }
//	}

//    void OnParticleCollision(GameObject other) {
//        int safeLength = system.GetSafeCollisionEventSize();
//        if (collisionEvents.Length < safeLength) {
//            Array.Resize(ref collisionEvents, safeLength);
//        }
//        int eventCount = system.GetCollisionEvents(other, collisionEvents);
//    }

//    public override bool CanFire(WeaponFiringParameters firingParameters) {
//        float lastFireTime = firingParameters.lastFireTime;
//        return Time.time - lastFireTime > fireRate;
//    }
//}

//#endif