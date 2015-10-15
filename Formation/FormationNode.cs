using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class FormationNode : MonoBehaviour,  IComparable {

    public float radius = 2f;

    protected Vector3 target;
    protected Vector3 originalPosition;

    public Formation formation;

    void Start() {
        originalPosition = transform.localPosition;
        target = Random.insideUnitSphere * radius;
    }

    void Update() {
        if ((originalPosition + target - transform.localPosition).sqrMagnitude < 1f) {
            target = Random.insideUnitSphere * radius;
        }
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition + target, 0.25f *  Time.deltaTime);
        }
    }

    public int CompareTo(object obj) {
        FormationNode other = obj as FormationNode;
        if (int.Parse(name) > int.Parse(other.name)) return 1;
        return -1;
    }
}
