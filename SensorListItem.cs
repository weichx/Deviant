using UnityEngine;
using UnityEngine.UI;

public class SensorListItem : MonoBehaviour {

    public Entity entity;
    protected Text entityNameText;
    protected Text entityDistanceText;
    protected Text entityDistanceUnitText;
    protected Image selectionImage;
    protected bool selected;

	void Start () {
        selectionImage = GetComponent<Image>();
        entityNameText = transform.Find("Entity Name").GetComponent<Text>();
        entityDistanceText = transform.Find("Entity Distance").GetComponent<Text>();
        entityDistanceUnitText = transform.Find("Entity Distance Unit").GetComponent<Text>();
        //on entity faction changed => change color
        if(entity != null) {
            entityNameText.color = FactionManager.GetColor(entity.factionId);
            entityNameText.text = entity.displayName;
            selectionImage.enabled = selected;
        }
    }
	
	void Update () {
        if (entity == null) return;
        float distance = Vector3.Distance(entity.transform.position, PlayerManager.PlayerViewpointTransform.position);
        if(distance < 1000f) {
            entityDistanceUnitText.text = "M";
            entityDistanceText.text = ((int)distance).ToString();
        }
        else {
            entityDistanceText.text = (distance * 0.001).ToString("0.00");
            entityDistanceUnitText.text = "KM";
        }
    }

    public void SetEntity(Entity entity, bool selected = false) {
        this.entity = entity;
        this.selected = selected;
        if(entityDistanceText != null) {
            entityNameText.text = entity.displayName;
            entityNameText.color = FactionManager.GetColor(entity.factionId);
        }
        if (selectionImage != null) {
            selectionImage.enabled = selected;
        }
    }

    public void Select() {
        selectionImage.enabled = true;
        this.selected = true;
    }

    public void Deselect() {
        selectionImage.enabled = false;
        this.selected = false;
    }
}
