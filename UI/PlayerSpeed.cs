using UnityEngine;
using UnityEngine.UI;

public class PlayerSpeed : MonoBehaviour {
    protected Entity entity;
    protected Text textElement;

    void Start() {
        entity = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        textElement = GetComponent<Text>();
    }

    public void Update() {
        textElement.text = ((int)(entity.speed)).ToString();
    }
}