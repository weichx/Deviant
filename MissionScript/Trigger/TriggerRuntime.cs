using UnityEngine;
using System.Collections.Generic;

public class TriggerRuntime : MonoBehaviour {
    private static List<Trigger> triggers;

    public static void AddTrigger(Trigger trigger) {
        if (triggers == null) {
            triggers = new List<Trigger>();
        }
        triggers.Add(trigger);
    }

    public void Update() {
        for (int i = 0; i < TriggerRuntime.triggers.Count; i++) {
            if (TriggerRuntime.triggers[i].Run() != TriggerStatus.Pending) {
                TriggerRuntime.triggers.RemoveAt(i--);
            }
        }
    }
}
