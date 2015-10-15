using UnityEngine;

public class PlayerController : MonoBehaviour {

    private EngineSystem engines;
    private FlightControls flightControls;
    private WeaponSystem weaponSystem;

    public float yawDamp = 0.5f;
    public float pitchDamp = 1;
    public float rollDamp = 2;
    public float startThrottle = 1f;
    public Entity target;
    private CameraEye cameraEye;
    private bool focused = true;

    public void Start() {
        engines = GetComponent<EngineSystem>();
        weaponSystem = GetComponent<WeaponSystem>();
        flightControls = new FlightControls(yawDamp, pitchDamp, rollDamp);
        engines.SetFlightControls(flightControls);
        flightControls.SetStickInputs(0f, 0f, 0f, startThrottle);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
        PlayerManager.PlayerEventManager.AddListener<Event_EntityDamaged>(OnDamageTaken);
        cameraEye = GetComponentInChildren<CameraEye>();
    }

    public void Update() {
        if(!focused) {
            flightControls.SetStickInputs(0f, 0f, 0f, 0f);
            return;
        }
        float x = Input.GetAxis("JoystickX");
        float y = Input.GetAxis("JoystickY");
        float z = Input.GetAxis("JoystickZ");
        float t = (Input.GetAxisRaw("JoystickThrottle") + 1) * 0.5f;
        bool fire = Input.GetButton("Fire1");

        flightControls.SetStickDamping(yawDamp, pitchDamp, rollDamp);
        flightControls.SetStickInputs(x, y, z, t);
        if (fire && weaponSystem != null) {
            weaponSystem.weaponGroups[0].Fire();
        }

    }

    public void OnApplicationFocus(bool focused) {
        this.focused = focused;
    }

    protected void OnEntityDespawned(Event_EntityDespawned evt) {
        if(target == evt.entity) {
            EventManager.Instance.TriggerEvent(new Event_PlayerTargetChanged(null, evt.entity));
        }
    }

    protected void OnDamageTaken(Event_EntityDamaged evt) {
        cameraEye.Shake(0.05f);
    }

}
