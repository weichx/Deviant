using UnityEngine;

public class VisiblitityTracker : MonoBehaviour {
    [HideInInspector]
    public Entity entity;
    protected Event_EntityVisibilityChanged evt;

    public void Start() {
        evt = new Event_EntityVisibilityChanged(entity);
    }

    public void OnBecameVisible() {
        if(entity) {
            evt.isVisible = true;
            EventManager.Instance.QueueEvent(evt);
        }    
    }

    public void OnBecameInvisible() {
        if(entity) {
            evt.isVisible = false;
            //when exiting the manager dies but th
            if (EventManager.Instance) {
                EventManager.Instance.QueueEvent(evt);
            }
        }
    }
}