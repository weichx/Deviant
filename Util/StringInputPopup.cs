
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class StringInputPopup : EditorWindow {
    private string input = string.Empty;

    public delegate void OnInputSet(string groupName);
    public OnInputSet onInputSet;

    public string prompt = "Input";

    public void OnGUI() {
        input = EditorGUILayout.TextField(prompt, input);
        Event e = Event.current;

        if (input.Length > 0 && (e.keyCode == KeyCode.Return || GUILayout.Button("Ok"))) {
            if (onInputSet != null) {
                onInputSet(input);
            }
            Close();
        }

        if (GUILayout.Button("Cancel")) {
            Close();
        }
    }

}

#endif