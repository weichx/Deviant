using UnityEngine;
using UnityEngine.UI;

public class PlayerDistanceToTarget : MonoBehaviour {

    protected Text textElement;
    protected Text unitTextElement;

    public Transform target;

    public void Start() {
        textElement = GetComponent<Text>();
        unitTextElement = transform.Find("Unit").GetComponent<Text>();
        textElement.gameObject.SetActive(target != null);
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);
    }

    public void LateUpdate() {
        if (target == null) return;
        float distance = Vector3.Distance(Camera.main.transform.position, target.position);
        textElement.text = (distance * 0.001).ToString("0.00");
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        if (evt.newTarget != null) {
            target = evt.newTarget.transform;
            Color color = FactionManager.GetColor(evt.newTarget.factionId);
            textElement.color = color;
            unitTextElement.color = color;
        }
        else {
            target = null;
        }
        textElement.gameObject.SetActive(target != null);
    }
}
