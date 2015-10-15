using UnityEngine;
using System.Collections.Generic;

public class FactionView {
    public FactionId id;
    public List<Faction> relations;
}

[System.Serializable]
public class Faction {

    [HideInInspector]
    public FactionId id;
    [HideInInspector]
    public List<Entity> hostiles;
    [HideInInspector]
    public List<Entity> neutrals;
    [HideInInspector]
    public List<Entity> friendlies;
    private Dictionary<FactionId, Disposition> dispositions;

    public Faction(FactionId id, Dictionary<FactionId, Disposition> dispositions) {
        this.id = id;
        this.dispositions = dispositions;
        this.dispositions[id] = Disposition.Friendly; //Set self to friendly
        Reset();
    }

    public void Reset() {
        hostiles = new List<Entity>();
        neutrals = new List<Entity>();
        friendlies = new List<Entity>();
    }

    public bool IsEntityHostile(Entity ent) {
        return dispositions[ent.factionId] == Disposition.Hostile;
    }

    public bool IsEntityFriendly(Entity ent) {
        return dispositions[ent.factionId] == Disposition.Friendly;
    }

    public bool IsEntityNeutral(Entity ent) {
        return dispositions[ent.factionId] == Disposition.Neutral;
    }

    public void SetFactionFriendly(FactionId factionId) {
        Disposition disp = dispositions[factionId];
        if (factionId == id || disp == Disposition.Friendly) {
            return;

        } else if (disp == Disposition.Hostile) {

            for (var i = 0; i < hostiles.Count; i++) {
                if (hostiles[i].factionId == factionId) {
                    friendlies.Add(hostiles[i]);
                    hostiles.RemoveAt(i);
                    i--;
                }
            }

        } else {
            for (var i = 0; i < neutrals.Count; i++) {
                if (neutrals[i].factionId == factionId) {
                    friendlies.Add(neutrals[i]);
                    neutrals.RemoveAt(i);
                    i--;
                }
            }
        }
        dispositions[factionId] = Disposition.Friendly;

    }

    public void SetFactionNeutral(FactionId factionId) {
        Disposition disp = dispositions[factionId];
        if (factionId == id || disp == Disposition.Neutral) {
            return;

        } else if (disp == Disposition.Hostile) {

            for (var i = 0; i < hostiles.Count; i++) {
                if (hostiles[i].factionId == factionId) {
                    neutrals.Add(hostiles[i]);
                    hostiles.RemoveAt(i);
                    i--;
                }
            }

        } else {
            for (var i = 0; i < friendlies.Count; i++) {
                if (friendlies[i].factionId == factionId) {
                    neutrals.Add(friendlies[i]);
                    friendlies.RemoveAt(i);
                    i--;
                }
            }
        }
        dispositions[factionId] = Disposition.Neutral;
    }

    public void SetFactionHostile(FactionId factionId) {
        Disposition disp = dispositions[factionId];
        if (factionId == id || disp == Disposition.Hostile) {
            return;

        } else if (disp == Disposition.Friendly) {

            for (var i = 0; i < friendlies.Count; i++) {
                if (friendlies[i].factionId == factionId) {
                    hostiles.Add(friendlies[i]);
                    friendlies.RemoveAt(i);
                    i--;
                }
            }

        } else {
            for (var i = 0; i < neutrals.Count; i++) {
                if (neutrals[i].factionId == factionId) {
                    hostiles.Add(neutrals[i]);
                    neutrals.RemoveAt(i);
                    i--;
                }
            }
        }
        dispositions[factionId] = Disposition.Hostile;
    }

    public void AddEntity(Entity entity) {
        switch (dispositions[entity.factionId]) {
            case Disposition.Friendly:
                friendlies.Add(entity);
                break;
            case Disposition.Hostile:
                hostiles.Add(entity);
                break;
            case Disposition.Neutral:
                neutrals.Add(entity);
                break;
        }
    }

    public bool RemoveEntity(Entity entity) {
        switch (dispositions[entity.factionId]) {
            case Disposition.Friendly:
                return friendlies.Remove(entity);
            case Disposition.Hostile:
                return hostiles.Remove(entity);
            case Disposition.Neutral:
                return neutrals.Remove(entity);
            default:
                return false;
        }
    }

    public void SetDisposition(FactionId other, Disposition disposition) {
        switch (disposition) {
            case Disposition.Friendly:
                SetFactionFriendly(other);
                break;
            case Disposition.Hostile:
                SetFactionHostile(other);
                break;
            case Disposition.Neutral:
                SetFactionNeutral(other);
                break;
        }
    }

    public List<Entity> GetHostiles() {
        return  new List<Entity>(hostiles);
    }

    public List<Entity> GetFriendlies() {
        return new List<Entity>(friendlies);
    }

    public int HostileCount {
        get { return hostiles.Count; }
    }
}
