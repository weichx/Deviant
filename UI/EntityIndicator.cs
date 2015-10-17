using UnityEngine;
using UnityEngine.UI;

public class EntityIndicator : WorldPositionUI {

    protected Image indicatorImage;
    public Entity entity;

    public void Awake() {
        indicatorImage = GetComponent<Image>();
    }

    public void SetColor(Color color) {
        indicatorImage.color = color;
    }
}