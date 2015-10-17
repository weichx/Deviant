﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class WorldPositionUI : MonoBehaviour {
    public Transform trackedTransform;
    protected Vector3? trackedPosition;
    protected Graphic[] graphics;
    protected RectTransform rectTransform;

    private static float guiScale = 1.0f;
    private static bool isScaleInitialized;

    public virtual void Start() {
        rectTransform = GetComponent<RectTransform>();
        graphics = GetComponentsInChildren<Graphic>();
        if (!isScaleInitialized) {
            var scaler = GetComponentInParent<CanvasScaler>();
            Assert.IsNotNull(scaler, "Missing CanvasScaler Component");
            if (Mathf.Approximately(scaler.matchWidthOrHeight, 0.0f)) {
                guiScale = scaler.referenceResolution.x / (float)Screen.width;
            }
            else if (Mathf.Approximately(scaler.matchWidthOrHeight, 1.0f)) {
                guiScale = scaler.referenceResolution.y / (float)Screen.height;
            }
            else {
                Debug.LogWarning("Canvas scales between width and height are not supported by WorldPositionUI");
            }
            isScaleInitialized = true;
        }
        LateUpdate();
    }

    public virtual void LateUpdate() {
        if (trackedTransform == null && trackedPosition == null) {
            for (int i = 0; i < graphics.Length; i++) {
                graphics[i].enabled = false;
            }
            return;
        }

        Vector3 position;

        if(trackedTransform) {
            position = trackedTransform.position;
        }
        else {
            position = (Vector3)trackedPosition;
        }

        Assert.IsNotNull(Camera.main, "Camera is null");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        bool visible = screenPos.z > 0.0f;
        for (int i = 0; i < graphics.Length; i++) {
            graphics[i].enabled = visible;
        }
        rectTransform.anchoredPosition = new Vector2(guiScale * screenPos.x, guiScale * screenPos.y);
    }

    public void SetTrackedPosition(Vector3? position) {
        trackedPosition = position;
        trackedTransform = null;
    }

    public void SetTrackedObject(GameObject target) {
        if(target != null && target.transform != trackedTransform) {
            trackedTransform = target.transform;
            trackedPosition = null;
        }
    }

    public void SetTrackedObject(Transform target) {
        if (target != trackedTransform) {
            trackedTransform = target;
            trackedPosition = null;
        }
    }

    public void SetTrackedObject(Entity entity) {
        if (entity != null && trackedTransform != entity.transform) {
            trackedTransform = entity.transform;
            trackedPosition = null;
        }
    }

    public void Untrack() {
        trackedTransform = null;
        trackedPosition = null;
        for (int i = 0; i < graphics.Length; i++) {
            graphics[i].enabled = false;
        }
    }
}