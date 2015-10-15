using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Entity))]
public class WeaponSystem : MonoBehaviour {
    public List<WeaponGroup> weaponGroups;
    public List<Firepoint> firepoints;
    private Entity entity;

    private void Awake() {
        CollectWeaponGroups();
    }

    public void CollectWeaponGroups() {
        firepoints = new List<Firepoint>(transform.GetComponentsInChildren<Firepoint>(true));
        weaponGroups = new List<WeaponGroup>(transform.GetComponentsInChildren<WeaponGroup>(true));
        EnsureDefaultWeaponGroups();
        for (int i = 0; i < firepoints.Count; i++) {
            string groupId = firepoints[i].weaponGroupId;
            bool found = false;
            for (int j = 0; j < weaponGroups.Count; j++) {
                if (weaponGroups[j].groupId == groupId) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                firepoints[i].weaponGroupId = WeaponGroup.DefaultId;
            }
        }
    }

    protected void EnsureDefaultWeaponGroups() {
        if (weaponGroups.Count == 0) {
            AddWeaponGroup(WeaponGroup.DefaultId);
            for (int i = 0; i < firepoints.Count; i++) {
                firepoints[i].weaponGroupId = WeaponGroup.DefaultId;
            }
            return;
        }
        for (int i = 0; i < weaponGroups.Count; i++) {
            if (weaponGroups[i].groupId == WeaponGroup.DefaultId) {
                return;
            }
        }
        AddWeaponGroup(WeaponGroup.DefaultId);
    }

    public void AddFirepoint() {
        Transform weaponRoot = transform.FindChild("weapon_root");
        if (weaponRoot == null) {
            GameObject root = new GameObject();
            root.transform.parent = transform;
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            weaponRoot = root.transform;
            weaponRoot.name = "weapon_root";
        }
        GameObject firepoint = new GameObject();
        firepoint.transform.parent = weaponRoot;
        firepoint.transform.localPosition = Vector3.zero;
        firepoint.transform.localRotation = Quaternion.identity;
        firepoint.AddComponent<Firepoint>();
        firepoint.name = "Firepoint " + firepoints.Count;
        firepoints.Add(firepoint.GetComponent<Firepoint>());
    }

    public void AddWeaponGroup(string groupId) {
        Transform weaponRootTransform = transform.Find("weapon_root");
        if (weaponRootTransform == null) {
            weaponRootTransform = new GameObject("weapon_root").transform;
            weaponRootTransform.parent = transform;
        }
        GameObject weaponRoot = weaponRootTransform.gameObject;
        WeaponGroup group = weaponRoot.AddComponent<WeaponGroup>();
        group.groupId = groupId;
        weaponGroups.Add(group);
    }

    public void RemoveWeaponGroup(string groupId) {
        WeaponGroup group = weaponGroups.Find((g) => {
            return g.groupId == groupId;
        });
        if (group != null) {
            weaponGroups.Remove(group);
            DestroyImmediate(group);

        }
    }
}