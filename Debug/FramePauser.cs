using UnityEngine;
using System.Collections;

public class FramePauser : MonoBehaviour {
    [Range(0, 1)]
    public float timeScale = 0.5f;
    public int frameCount = 10;
    public bool usePausing = false;
    public KeyCode advanceKeyCode = KeyCode.Space;
    private int elapsedFrames = 0;

    void Update() {
        if (usePausing) {
            elapsedFrames++;
            if (elapsedFrames > frameCount) {
                Time.timeScale = 0;
            }
        }
        if (Input.GetKeyDown(advanceKeyCode)) {
            Time.timeScale = timeScale;
            elapsedFrames = 0;
        }
    }
}
