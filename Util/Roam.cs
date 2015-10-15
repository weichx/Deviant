using UnityEngine;
using System.Collections;

public class Roam : MonoBehaviour {

    Timer timer;
    public float roamTimeOut = 5000f;
    public float speed = 2;
    public Vector3 target;

	void Start () {
        timer = new Timer(roamTimeOut);
        Vector3 pos = Random.insideUnitSphere;
        target = transform.position + (pos * Random.Range(10, 25));
    }
	
	// Update is called once per frame
	void Update () {
        float distanceToTarget = Vector3.Distance(transform.position, target);
        if (timer.Ready || distanceToTarget < 1) {
            Vector3 pos = Random.insideUnitSphere;
            target = transform.position + (pos * Random.Range(10, 100));
            timer.Reset(5000);
        }
        var toTarget = target - transform.position;
        transform.position += toTarget.normalized * speed * Time.deltaTime;
	}
}
