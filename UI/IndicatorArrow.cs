using UnityEngine;
using UnityEngine.UI;

public class IndicatorArrow : MonoBehaviour {

    public Transform target;
    public float distanceFromCenter = 100f;
    public float visibleDistance = 300f;

    protected RectTransform rectTransform;
    protected Image image;

    public void Start() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        image.gameObject.SetActive(target != null);
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);
    }

    public void LateUpdate() {
        if (target == null) {
            return;
        }

        Camera camera = Camera.main;
        Vector3 cameraPosition = camera.transform.position;
        Vector3 targetPosition = target.transform.position;
        Transform cameraTransform = camera.transform;

        Vector3 toTarget = cameraPosition.DirectionTo(targetPosition);
        Vector3 projected = Vector3.ProjectOnPlane(toTarget, cameraTransform.forward);

        float angle = Util.AngleSigned(cameraTransform.up, projected, cameraTransform.forward);
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);

        Vector3 position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
        Vector3 screenPosition = camera.WorldToScreenPoint(targetPosition);

        float distance2d = Vector2.Distance(position, screenPosition);
        image.enabled = distance2d >= visibleDistance || screenPosition.z < 0f;

        position += (rectTransform.up * distanceFromCenter);
        position.z = 0;
        rectTransform.position = position;
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        target = null;
        if (evt.newTarget) {
            target = evt.newTarget.transform;
            image.color = FactionManager.GetColor(evt.newTarget.factionId);
        }
        image.gameObject.SetActive(target != null);
    }
}
