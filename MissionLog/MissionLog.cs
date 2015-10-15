using System;
using System.Collections.Generic;

public enum MissionLogItemType {
    Destroyed, Entered, Exited, Disabled, Boarded, Repaired, MissionEvent
}

public class MissionLogEntry {
    public float timestamp;
    public MissionLogItemType type;
    public string regarding;
    public object data;
}

public class MissionLog {
    public void EntityDestroyed(Entity e) {
        //DestroyedEntities.Add(e.id)
    }

    public bool IsEntityDestroyed(string entityId) {
        return true;
    }

    public bool DidEntityEnter(string entityId) {
        return true;
    }

    public bool DidEntityDepart(string entityId) {
        return false;
    }

    public bool IsEntityDisabled(string entityId) {
        return false;
    }

    public bool WasEntityDisabled(string entitId) {
        return false;
    }

    public static bool EventFired(string eventName) {
        return true;
    }
}

