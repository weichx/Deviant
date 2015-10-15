using UnityEngine;
using UnityEngine.UI;

public class PlayerTargetSpeed : MonoBehaviour {

    protected Text textElement;
    public Entity target;

    public void Start() {
        textElement = GetComponent<Text>();
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);
        textElement.gameObject.SetActive(target != null);
    }

    public void Update() {
        if (target == null) return;
        textElement.text = target.speed.ToString(); 
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        if(evt.newTarget) {
            target = evt.newTarget;
            textElement.gameObject.SetActive(true);
        }
        else {
            textElement.gameObject.SetActive(false);
        }
    }
}