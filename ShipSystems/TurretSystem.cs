using System.Collections.Generic;
using UnityEngine;

public class TurretSystem : MonoBehaviour {
    public List<Turret> turrets;
    public List<TurretGroup> turretGroups;

    //todo consider changing turret to only have an AlignTo method & helpers
    //let an AIPilot or AITurret actually control it
    public Transform tempTarget;

    void Awake() {
        CollectTurrets();
        for (int i = 0; i < turretGroups.Count; i++) {
            TurretGroup group = turretGroups[i];
            for (int j = 0; j < turrets.Count; j++) {
                Turret turret = turrets[j];
                if (turret.turretGroupId == group.groupId) {
                    turret.weaponId = group.weaponId;
                }
            }
        }
    }

    public void SetAllTurretsTarget() {
        for (int i = 0; i < turrets.Count; i++) {
            turrets[i].target = tempTarget;
        }
    }

    public TurretGroup GetGroup(string groupId) {
        for (int i = 0; i < turretGroups.Count; i++) {
            if (turretGroups[i].groupId == groupId) return turretGroups[i];
        }
        return null;
    }

    public void AddTurretGroup(string groupId) {
        //todo ensure name is unique
        Transform turretRootTransform = transform.Find("turret_root");
        if (turretRootTransform == null) {
            turretRootTransform = new GameObject("turret_root").transform;
            turretRootTransform.parent = transform;
        }
        GameObject turretRoot = turretRootTransform.gameObject;
        TurretGroup group = turretRoot.AddComponent<TurretGroup>();
        group.groupId = groupId;
        turretGroups.Add(group);
    }

    public void CollectTurrets() {
        turretGroups = new List<TurretGroup>(transform.GetComponentsInChildren<TurretGroup>(true));
        turrets = new List<Turret>(transform.GetComponentsInChildren<Turret>(true));
        EnsureDefaultTurretGroup();

        for (int i = 0; i < turrets.Count; i++) {
            string groupId = turrets[i].turretGroupId;
            bool found = false;
            for (int j = 0; j < turretGroups.Count; j++) {
                if (turretGroups[j].groupId == groupId) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                turrets[i].turretGroupId = TurretGroup.DefaultId;
            }
        }
    }

    private void EnsureDefaultTurretGroup() {
        if (turretGroups.Count == 0) {
            AddTurretGroup(TurretGroup.DefaultId);
            for (int i = 0; i < turrets.Count; i++) {
                turrets[i].turretGroupId = TurretGroup.DefaultId;
            }
            return;
        }

        for (int i = 0; i < turretGroups.Count; i++) {
            if (turretGroups[i].groupId == TurretGroup.DefaultId) {
                return;
            }
        }
        AddTurretGroup(TurretGroup.DefaultId);
    }

    public void LockAllTurrets() {
        if (turrets == null) return;
        for (int i = 0; i < turrets.Count; i++) {
            turrets[i].locked = false;
        }
    }

    public void UnlockAllTurrets() {
        if (turrets == null) return;
        for (int i = 0; i < turrets.Count; i++) {
            turrets[i].locked = false;
        }
    }
}


