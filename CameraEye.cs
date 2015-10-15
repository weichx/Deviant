using UnityEngine;

public class CameraEye : MonoBehaviour {

    public float shakeDecay = 0.002f;
    public float maxShakeIntensity = 0.1f;
    public float shakeMultiplier = 0.2f;
    public float recoveryTime = 1.5f;

    private float intensity;
    private Timer timer;
    private Quaternion originalRotation;
    private Quaternion lastIntensityRotation;
    private Entity entity;

    public void Start() {
        timer = new Timer();
        entity = PlayerManager.PlayerEntity;
    }

    public void Update() {
        if (intensity > 0f) {
            transform.localRotation = new Quaternion(
                originalRotation.x + Random.Range(-intensity, intensity) * shakeMultiplier,
                originalRotation.y + Random.Range(-intensity, intensity) * shakeMultiplier,
                originalRotation.z + Random.Range(-intensity, intensity) * shakeMultiplier,
                originalRotation.w + Random.Range(-intensity, intensity) * shakeMultiplier
            );
            intensity -= shakeDecay;
            intensity = Mathf.Clamp(intensity, 0, maxShakeIntensity);
            if(intensity == 0) {
                timer.Reset(recoveryTime);
                lastIntensityRotation = transform.localRotation;
            }
        } else {
            if (!timer.Ready) {
                transform.localRotation = Quaternion.Slerp(lastIntensityRotation, Quaternion.identity, timer.ElapsedTime / recoveryTime);
            }
            else {
                transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void Shake(float intensity) {
        this.intensity += intensity;
        intensity = Mathf.Clamp(intensity, 0, maxShakeIntensity);
        originalRotation = transform.localRotation;
    }

    void OnGUI() {
        if(GUI.Button(new Rect(20, 40, 80, 20), "Shake")) {
            Shake(0.05f);
        }
    }

}
