#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(WeaponSystem))]
public class WeaponSystemEditor : Editor {
    private ReorderableList firepointList;
    private ReorderableList groupList;
    private WeaponSystem weaponSystem;
    private List<string> weaponGroupNames;
    private Firepoint selectedFirepoint;

    private void OnEnable() {
        weaponSystem = target as WeaponSystem;
        weaponSystem.CollectWeaponGroups();
        firepointList = new ReorderableList(serializedObject, serializedObject.FindProperty("firepoints"), true, true, true, true);
        firepointList.drawElementCallback += DrawFirepoint;
        firepointList.drawHeaderCallback += DrawHeaderCallback;
        firepointList.onAddCallback += AddFirepoint;
        firepointList.onSelectCallback += SelectFirepoint;

        groupList = new ReorderableList(serializedObject, serializedObject.FindProperty("weaponGroups"), true, true, true, true);
        groupList.elementHeight = EditorGUIUtility.singleLineHeight;
        groupList.drawElementCallback += DrawWeaponGroup;
        groupList.drawHeaderCallback += DrawWeaponGroupHeader;
        groupList.onAddCallback += AddGroup;
        groupList.onCanRemoveCallback += CanRemoveGroup;
        groupList.onRemoveCallback += RemoveGroup;
        weaponGroupNames = new List<string>();
        for (int i = 0; i < weaponSystem.weaponGroups.Count; i++) {
            weaponGroupNames.Add(weaponSystem.weaponGroups[i].groupId);
        }
    }

    private void OnDisable() {
        groupList.drawElementCallback -= DrawWeaponGroup;
        groupList.drawHeaderCallback -= DrawWeaponGroupHeader;
        groupList.onAddCallback -= AddGroup;
        groupList.onCanRemoveCallback -= CanRemoveGroup;
        groupList.onRemoveCallback -= RemoveGroup;
        firepointList.drawElementCallback -= DrawFirepoint;
        firepointList.drawHeaderCallback -= DrawHeaderCallback;
        firepointList.onAddCallback -= AddFirepoint;
        firepointList.onSelectCallback -= SelectFirepoint;
        selectedFirepoint = null;
    }

    public void OnSceneGUI() {
        if (selectedFirepoint != null) {
            Handles.ArrowCap(0,
                    selectedFirepoint.transform.position,
                    selectedFirepoint.transform.rotation,
                    1);
        }
    }

    private void AddFirepoint(ReorderableList list) {
        weaponSystem.AddFirepoint();
    }

    private void SelectFirepoint(ReorderableList list) {
        Firepoint firepoint = weaponSystem.firepoints[list.index];
        if (firepoint != null) {
            EditorGUIUtility.PingObject(firepoint);
            selectedFirepoint = firepoint;
        }
    }

    private void AddGroup(ReorderableList list) {
        StringInputPopup window = (StringInputPopup)EditorWindow.GetWindow(typeof(StringInputPopup));
        window.onInputSet = (string groupName) => {
            int idx = weaponGroupNames.IndexOf(groupName);
            if (idx != -1) {
                groupName += "(1)"; //todo this is terrible
            }
            weaponSystem.AddWeaponGroup(groupName);
            EditorUtility.SetDirty(weaponSystem);
            weaponGroupNames.Add(groupName);
        };

        window.position = new Rect((Screen.width / 2) - 125, Screen.height / 2, 250, 55);
        window.ShowUtility();
    }

    private void RemoveGroup(ReorderableList list) {
        WeaponGroup group = weaponSystem.weaponGroups[list.index];
        if (group == null) return;
        if (group.groupId == WeaponGroup.DefaultId) {
            Debug.Log("Default group cannot be removed");
        } else {
            for (int i = 0; i < weaponSystem.firepoints.Count; i++) {
                Firepoint firepoint = weaponSystem.firepoints[i];
                if (firepoint.weaponGroupId == group.groupId) {
                    firepoint.weaponGroupId = WeaponGroup.DefaultId;
                }
            }
            weaponGroupNames.Remove(group.groupId);
            weaponSystem.RemoveWeaponGroup(group.groupId);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }
    }

    private bool CanRemoveGroup(ReorderableList list) {
        if (weaponSystem.weaponGroups[list.index] == null) return true;
        return weaponSystem.weaponGroups[list.index].groupId != WeaponGroup.DefaultId && weaponSystem.weaponGroups.Count > 1;
    }

    private void DrawWeaponGroup(Rect rect, int index, bool isActive, bool isFocused) {
        float width = EditorGUIUtility.currentViewWidth;
        float height = EditorGUIUtility.singleLineHeight;
        float halfWidth = (width * 0.5f) - 25f;
        float quaterWidth = halfWidth * 0.5f;

        WeaponGroup group = weaponSystem.weaponGroups[index];
        if (group == null) return;
        int idx = WeaponDatabase.GetWeaponIndex(group.weaponId);
        if (idx == -1) {
            Debug.Log("Cannot find weapon: " + group.weaponId);
            idx = 0;
        }
        int count = weaponSystem.firepoints.FindAll((firepoint) => firepoint.weaponGroupId == group.groupId).Count;

        Rect r = new Rect(rect.x, rect.y, quaterWidth + (quaterWidth * 0.5f), height);
        EditorGUI.LabelField(r, group.groupId + "(" + count + ")");

        r = new Rect(rect.x + quaterWidth + (quaterWidth * 0.5f), rect.y, halfWidth, height);
        int newIdx = EditorGUI.Popup(r, idx, WeaponDatabase.GetWeaponList());
        if (idx != newIdx) {
            group.weaponId = WeaponDatabase.GetWeaponList()[newIdx];
            EditorUtility.SetDirty(group);
        }
    }

    private void DrawWeaponGroupHeader(Rect rect) {
        EditorGUI.LabelField(rect, "Weapon Groups");
    }

    private void DrawFirepoint(Rect rect, int index, bool isActive, bool isFocused) {
      //  var element = firepointList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        Rect r = new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth * 0.33f, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(r, new GUIContent(weaponSystem.firepoints[index].name));
        r.x += r.width;
        r.width = 100f;
        Firepoint firepoint = weaponSystem.firepoints[index];
        int groupIdx = weaponGroupNames.IndexOf(firepoint.weaponGroupId);
        if (groupIdx == -1) {
            Debug.Log("Cant find group: " + firepoint.weaponGroupId + ", reseting to default group");
            groupIdx = 0;
        }
        groupIdx = EditorGUI.Popup(r, groupIdx, weaponGroupNames.ToArray());
        firepoint.weaponGroupId = weaponGroupNames[groupIdx];
    }

    private void DrawHeaderCallback(Rect rect) {
        EditorGUI.LabelField(rect, "Weapons");
    }

    public override void OnInspectorGUI() {
        weaponSystem = target as WeaponSystem;
        serializedObject.Update();
        groupList.DoLayoutList();
        firepointList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        
        if (GUILayout.Button("Refresh Firepoints")) {
            weaponSystem.CollectWeaponGroups();
        }
        if (GUILayout.Button("Sort Firepoints")) {
            weaponSystem.firepoints.Sort(delegate(Firepoint a, Firepoint b) {
                return a.name.CompareTo(b.name);
            });
        }
    }
}

#endif