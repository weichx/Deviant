using UnityEngine;

public class Gunbank : MonoBehaviour {
    public string weaponId;
    public bool linkable;
    public int ammunition;

    private int currentHardpoint;
    private Transform[] hardpoints;

    void Awake() {
        CollectHardpoints();
    }
    
    public int HardpointCount {
        get { return hardpoints.Length; }
    }

    public Transform NextHardpoint {
        get {
            if (hardpoints.Length == 0) return null;
            currentHardpoint++;
            if (currentHardpoint == hardpoints.Length) {
                currentHardpoint = 0;
            }
            return hardpoints[currentHardpoint];
        }
    }

    public Transform[] CollectHardpoints() {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++) {
            children[i] = transform.GetChild(i);
        }
        hardpoints = children;
        return children;
    }

    public void Reset() {
        name = "Gunbank";
    }
}