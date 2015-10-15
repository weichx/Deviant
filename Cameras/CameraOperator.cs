using UnityEngine;
using System.Collections;

public class CameraOperator : MonoBehaviour {
    public Texture2D selectionHighlight = null;
    public static Rect selection = new Rect(0, 0, 0, 0);
    private Vector3 startClick = -Vector3.one;

    // Update is called once per frame
    void Update() {
        CheckCamera();
    }

    public static bool WithinSelection(Vector3 point) {
        Vector3 camPos = Camera.main.WorldToScreenPoint(point);
        camPos.y = CameraOperator.InvertMouseY(camPos.y);
        return CameraOperator.selection.Contains(camPos);
    }

    private void CheckCamera() {
        if (Input.GetMouseButtonDown(0)) {
            startClick = Input.mousePosition;

        } else if (Input.GetMouseButtonUp(0)) {
            startClick = -Vector3.one;
        }

        if (Input.GetMouseButton(0)) {
            var x = startClick.x;
            var y = InvertMouseY(startClick.y);
            var w = Input.mousePosition.x - startClick.x;
            var h = InvertMouseY(Input.mousePosition.y) - InvertMouseY(startClick.y);
            selection = new Rect(x, y, w, h);
            if (selection.width < 0) {
                selection.x += selection.width;
                selection.width = -selection.width;
            }
            if (selection.height < 0) {
                selection.y += selection.height;
                selection.height = -selection.height;
            }
        }
    }

    private void OnGUI() {
        if (startClick != -Vector3.one) {
            GUI.color = new Color(1, 1, 1, 0.5f);
            GUI.DrawTexture(selection, selectionHighlight);
        }
    }

    public static float InvertMouseY(float y) {
        return Screen.height - y;
    }
}
