using UnityEngine;
using System.Collections;

public class SpeedHud : MonoBehaviour {
    public EngineSystem engineSystem;
    public UnityEngine.UI.Text textComponent;

	// Use this for initialization
	void Start () {
        textComponent = GetComponent<UnityEngine.UI.Text>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(engineSystem) {
            textComponent.text = "Speed: " + engineSystem.speed.ToString("f0");
        }
	}
}
