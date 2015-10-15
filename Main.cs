using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    public Transform followTarget;

    void Update () {
	    if (followTarget) {
	        transform.position = followTarget.position;
	    }
	}
}
