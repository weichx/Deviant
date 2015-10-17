﻿using UnityEngine;

public class PlayerPilot : Pilot {

    public float yawDamp = 0.5f;
    public float pitchDamp = 1;
    public float rollDamp = 2;
    public float startThrottle = 1f;

    private CameraEye cameraEye;
    private bool focused = true;

    public override void Start() {
        base.Start();
        flightControls.SetStickDamping(yawDamp, pitchDamp, rollDamp);
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

    protected override void OnEntityDespawned(Event_EntityDespawned evt) {
        base.OnEntityDespawned(evt);
        if (target == evt.entity) {
            EventManager.Instance.TriggerEvent(new Event_PlayerTargetChanged(null, evt.entity));
        }
    }

    protected void OnDamageTaken(Event_EntityDamaged evt) {
        cameraEye.Shake(0.05f);
    }

}
