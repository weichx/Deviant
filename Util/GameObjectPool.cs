using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool {
    //todo use a native array here
    //todo if x seconds go by below n threshold, cull y items
    private List<GameObject> activeList;
    private List<GameObject> inactiveList;

    public int initialSize;
    public int maximumSize;
    public GameObject prefab;
    private Transform parent;

    public GameObjectPool(GameObject prefab, int maximumSize, int initalSize, Transform parent) {
        this.prefab = prefab;
        this.initialSize = initalSize;
        this.maximumSize = maximumSize;
        this.activeList = new List<GameObject>(initalSize);
        this.inactiveList = new List<GameObject>(initalSize);
        this.parent = parent;
        if (maximumSize < 0) maximumSize = int.MaxValue;

        for (int i = 0; i < initialSize; i++) {
            GameObject poolObject = UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
           poolObject.hideFlags = HideFlags.HideInHierarchy;
            poolObject.SetActive(false);
            inactiveList.Add(poolObject);
            if (parent != null) {
                poolObject.transform.parent = parent;
            }
        }
    }

    public GameObject Spawn() {
        GameObject obj = null;
        if (inactiveList.Count > 0) {
            obj = inactiveList[inactiveList.Count - 1];
            inactiveList.RemoveAt(inactiveList.Count - 1);
        } else if (activeList.Count < maximumSize) {
            obj = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            obj.hideFlags = HideFlags.HideInHierarchy;
        } else {
            Debug.Log("Pool size exceeded for " + prefab.transform.name);
            return null;
        }
        obj.SetActive(true);
        activeList.Add(obj);
        obj.transform.parent = parent;
        return obj;
    }

    public void Despawn(GameObject obj) {
        activeList.Remove(obj);
        obj.transform.parent = parent;
        inactiveList.Add(obj);
        obj.SetActive(false);
    }

    public bool CanSpawn {
        get { return activeList.Count < maximumSize; }
    }
}
