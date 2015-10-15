using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class PlayerSensors : MonoBehaviour {

    protected Transform viewpoint;
    public float sensorRange = 2000f;
    public Entity target;
    protected List<Entity> targetList;
    protected int targetIndex;

    public void Start() {
        targetIndex = -1;
        targetList = new List<Entity>();
        CameraEye eye = GetComponentInChildren<CameraEye>();
        Assert.IsNotNull(eye, "Camera eye for player sensors was null");
        viewpoint = eye.transform;

        EventManager.Instance.AddListener<Event_EntitySpawned>(OnEntitySpawned);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
        
    }

    public void Update() {
        DoTargeting();       
    }

    protected void DoTargeting() {
        if (Input.GetKeyDown(KeyCode.T)) {
            CycleTargetUp();
            return;
        }

        if(Input.GetKeyDown(KeyCode.Y)) {
            CycleTargetDown();
            return;
        }

        if (Input.GetButtonDown("JoystickButton1")) {
            RaycastHit hit;
            if (Physics.SphereCast(viewpoint.transform.position, 15f, viewpoint.transform.forward, out hit)) {
                Entity entity = hit.transform.GetComponentInParent<Entity>();
                Assert.IsNotNull(entity, "Tried to target " + hit.transform.name + " but it doesnt have an entity");
                Target(entity);
            }
        }
    }

    protected void Target(Entity entity) {
        Entity oldTarget = target;
        target = entity;
        targetIndex = -1;

        if (entity != null) {
            for (var i = 0; i < targetList.Count; i++) {
                if (targetList[i] == entity) {
                    targetIndex = i;
                    break;
                }
            }
            Assert.AreNotEqual(targetIndex, -1, "Tried to target " + entity.displayName + " but it was not in the target list");
        }

        EventManager.Instance.QueueEvent(new Event_PlayerTargetChanged(target, oldTarget));
    }

    protected void ClearTarget() {
        target = null;
        targetIndex = -1;
    }

    protected void CycleTargetUp() {
        if (targetList.Count == 0) return;
        Target(targetList[(targetIndex + 1) % targetList.Count]);
    }

    protected void CycleTargetDown() {
        if (targetList.Count == 0) return;
        if (targetIndex == 0) targetIndex = targetList.Count;
        if (targetIndex == -1) targetIndex = 1;
        
        Target(targetList[targetIndex - 1]);
    }

    protected void OnEntityDespawned(Event_EntityDespawned evt) {
        if(evt.entity == target) {
            EventManager.Instance.TriggerEvent(new Event_PlayerTargetChanged(null, target));
            target = null;
        }
        targetList.Remove(evt.entity);
    }

    protected void OnEntitySpawned(Event_EntitySpawned evt) {
        if (evt.entity != PlayerManager.PlayerEntity) {
            targetList.Add(evt.entity);
        }
    }
}
