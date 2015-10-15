using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerTargetingBracket : WorldPositionUI {

    protected Color color;
    protected Image reticule;

    public override void Start() {
        base.Start();
        reticule = GetComponent<Image>();
        Assert.IsNotNull(reticule, "PlayerTargetingBracket is missing an image component");
        EventManager.Instance.AddListener<Event_PlayerTargetChanged>(OnPlayerTargetChanged);
    }

    public void OnPlayerTargetChanged(Event_PlayerTargetChanged evt) {
        SetTrackedObject(evt.newTarget);
        if(evt.newTarget != null) {
            reticule.color = FactionManager.GetColor(evt.newTarget.factionId);
        }
    }
}
