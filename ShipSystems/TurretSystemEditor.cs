#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(TurretSystem))]
public class TurretSystemEditor : Editor {
    private ReorderableList turretList;
    private ReorderableList groupList;
    private TurretSystem turretSystem;
    private List<string> turretGroupNames;

    private void OnEnable() {
        turretSystem = target as TurretSystem;
        turretSystem.CollectTurrets();
        turretList = new ReorderableList(serializedObject, serializedObject.FindProperty("turrets"), true, true, false, false);
        turretList.drawElementCallback += DrawTurretCallback;
        turretList.drawHeaderCallback += DrawHeaderCallback;
        turretList.onSelectCallback -= SelectTurret;
        groupList = new ReorderableList(serializedObject, serializedObject.FindProperty("turretGroups"), true, true, true, true);
        groupList.elementHeight = EditorGUIUtility.singleLineHeight;
        //groupList.elementHeight = 6 * EditorGUIUtility.singleLineHeight;
        groupList.drawElementCallback += DrawTurretGroup;
        groupList.drawHeaderCallback += DrawTurretGroupHeader;
        groupList.onAddCallback += AddGroup;
        groupList.onCanRemoveCallback += CanRemoveGroup;
        groupList.onRemoveCallback += RemoveGroup;
        turretGroupNames = new List<string>();
        for (int i = 0; i < turretSystem.turretGroups.Count; i++) {
            turretGroupNames.Add(turretSystem.turretGroups[i].groupId);
        }
    }

    private void OnDisable() {
        groupList.drawElementCallback -= DrawTurretGroup;
        groupList.drawHeaderCallback -= DrawTurretGroupHeader;
        groupList.onAddCallback -= AddGroup;
        groupList.onCanRemoveCallback -= CanRemoveGroup;
        groupList.onRemoveCallback -= RemoveGroup;
        turretList.drawElementCallback -= DrawTurretCallback;
        turretList.drawHeaderCallback -= DrawHeaderCallback;
        turretList.onSelectCallback -= SelectTurret;
    }

    private void SelectTurret(ReorderableList list) {
        EditorGUIUtility.PingObject(turretSystem.turrets[list.index]);
    }

    private void AddGroup(ReorderableList list) {
        StringInputPopup window = (StringInputPopup)EditorWindow.GetWindow(typeof(StringInputPopup));
        window.prompt = "Turret group name:";
        window.onInputSet = (string groupName) => {
            int idx = turretGroupNames.IndexOf(groupName);
            if (idx != -1) {
                groupName += "(1)"; //terrible
            }
            turretSystem.AddTurretGroup(groupName);
            EditorUtility.SetDirty(turretSystem);
            turretGroupNames.Add(groupName);
        };

        window.position = new Rect((Screen.width / 2) - 125, Screen.height / 2, 250, 55);
        window.ShowUtility();
    }

    private void RemoveGroup(ReorderableList list) {
        TurretGroup group = turretSystem.turretGroups[list.index];
        if (group.groupId == TurretGroup.DefaultId) {
            Debug.Log("Default group cannot be removed");
        } else {
            for (int i = 0; i < turretSystem.turrets.Count; i++) {
                Turret turret = turretSystem.turrets[i];
                if (turret.turretGroupId == group.groupId) {
                    turret.turretGroupId = TurretGroup.DefaultId;
                    EditorUtility.SetDirty(turret);
                }
            }
            turretGroupNames.Remove(group.groupId);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }
    }

    private bool CanRemoveGroup(ReorderableList list) {
        return turretSystem.turretGroups[list.index].groupId != TurretGroup.DefaultId && turretSystem.turretGroups.Count > 1;
    }

    private void DrawTurretGroup(Rect rect, int index, bool isActive, bool isFocused) {
        EditorGUI.BeginChangeCheck();
        float width = EditorGUIUtility.currentViewWidth;
        float height = EditorGUIUtility.singleLineHeight;
        float halfWidth = (width * 0.5f) - 25f;
        float quaterWidth = halfWidth * 0.5f;

        TurretGroup group = turretSystem.turretGroups[index];
        int idx = WeaponDatabase.GetWeaponIndex(group.weaponId);
        if (idx == -1) {
            Debug.Log("Cannot find weapon: " + group.weaponId);
            idx = 0;
        }
        int count = turretSystem.turrets.FindAll((turret) => turret.turretGroupId == group.groupId).Count;

        Rect r = new Rect(rect.x, rect.y, quaterWidth + (quaterWidth * 0.5f), height);
        EditorGUI.LabelField(r, group.groupId + "(" + count + ")");

        r = new Rect(rect.x + quaterWidth + (quaterWidth * 0.5f), rect.y, halfWidth, height);
        int newIdx = EditorGUI.Popup(r, idx, WeaponDatabase.GetWeaponList());
        if (newIdx != idx) {
            group.weaponId = (WeaponDatabase.GetWeaponList()[newIdx]);
            EditorUtility.SetDirty(group);
        }
    }

    private void DrawTurretGroupHeader(Rect rect) {
        EditorGUI.LabelField(rect, "Turret Groups");
    }

    private void DrawTurretCallback(Rect rect, int index, bool isActive, bool isFocused) {
       // var element = turretList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        Rect r = new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(r, new GUIContent(turretSystem.turrets[index].name));
        r.x += r.width;
        r.width = 100f;
        Turret turret = turretSystem.turrets[index];
        int groupIdx = turretGroupNames.IndexOf(turret.turretGroupId);
        if (groupIdx == -1) {
            Debug.Log("Cant find group: " + turret.turretGroupId + ", reseting to default group");
            groupIdx = 0;
        }
        int newIdx = EditorGUI.Popup(r, groupIdx, turretGroupNames.ToArray());
        if (newIdx != groupIdx) {
            turret.turretGroupId = turretGroupNames[newIdx];
            EditorUtility.SetDirty(turret);
        }
        
    }

    private void DrawHeaderCallback(Rect rect) {
        EditorGUI.LabelField(rect, "Turrets");
    }

    public override void OnInspectorGUI() {
        turretSystem = target as TurretSystem;
        serializedObject.Update();
        groupList.DoLayoutList();
        turretList.DoLayoutList();
        if (GUILayout.Button("Sort Turrets")) {
            turretSystem.turrets.Sort(delegate(Turret a, Turret b) {
                return a.name.CompareTo(b.name);
            });
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tempTarget"));
        if (GUILayout.Button("Set All Turrets Target")) {
            turretSystem.SetAllTurretsTarget();
        }
        serializedObject.ApplyModifiedProperties();

    }
}

#endif
//r = new Rect(rect.x + halfWidth + (quaterWidth * 0.75f), currentHeight - 2f, quaterWidth, height);
//if (GUI.Button(r, new GUIContent("Reset"))) {
//    group.Reset();
//}

//currentHeight += height + (height * 0.5f);

//r = new Rect(rect.x, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent("Range"));

//r = new Rect(rect.x + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.range = EditorGUI.FloatField(r, group.weaponData.range);

//r = new Rect(rect.x + halfWidth, currentHeight, quaterWidth, height);

//GUI.enabled = group.weaponData.lifeTime >= 0;
//EditorGUI.LabelField(r, new GUIContent(" Lifetime"));
//r = new Rect(rect.x + halfWidth + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.lifeTime = EditorGUI.FloatField(r, group.weaponData.lifeTime);
////todo clamp things so it cant be below 0 -- use BeginChangeCheck() / EndChangeCheck()
//GUI.enabled = true;

//currentHeight += height;

//r = new Rect(rect.x, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent("Fire Rate"));

//r = new Rect(rect.x + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.fireRate = EditorGUI.FloatField(r, group.weaponData.fireRate);

//GUI.enabled = group.weaponData.aspectTime >= 0;
//r = new Rect(rect.x + halfWidth, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent(" Lock Time"));

//r = new Rect(rect.x + halfWidth + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.aspectTime = EditorGUI.FloatField(r, group.weaponData.aspectTime);
//GUI.enabled = true;

//currentHeight += height;

//r = new Rect(rect.x, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent("Accuracy"));

//r = new Rect(rect.x + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.accuracy = EditorGUI.FloatField(r, group.weaponData.accuracy);

//r = new Rect(rect.x + halfWidth, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent(" Speed"));

//r = new Rect(rect.x + halfWidth + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.speed = EditorGUI.FloatField(r, group.weaponData.speed);

//currentHeight += height;

//r = new Rect(rect.x, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent("Hull"));

//r = new Rect(rect.x + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.hullDamage = EditorGUI.FloatField(r, group.weaponData.hullDamage);

//r = new Rect(rect.x + halfWidth, currentHeight, quaterWidth, height);
//EditorGUI.LabelField(r, new GUIContent(" Shield"));

//r = new Rect(rect.x + halfWidth + quaterWidth, currentHeight, quaterWidth, height);
//group.weaponData.shieldDamage = EditorGUI.FloatField(r, group.weaponData.shieldDamage);