using UnityEngine;
using System.Collections;

public class OneShotParticle : MonoBehaviour {
    protected ParticleSystem particles;

    void Start() {
        particles = GetComponent<ParticleSystem>();
    }

    void Update() {
        if(!particles.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
