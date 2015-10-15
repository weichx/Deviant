using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class FactionDispositionMap : SerializableDictionary<string, Disposition> { }

public class FactionManager : MonoBehaviour {

    public static List<Faction> factions;
    private static FactionManager instance;

    [HideInInspector]
    public FactionDispositionMap factionDispositionMap;
    private Dictionary<FactionId, Color> colorMap;

    void Awake() {
        if (instance != null) return;
        instance = this;
        factions = new List<Faction>();
        foreach (FactionId factionId in Enum.GetValues(typeof(FactionId))) {
            factions.Add(new Faction(factionId, CreateDispositionMap(factionId)));
        }
        SetFactionsHostile(FactionId.XWA, FactionId.Maas);
        EventManager.Instance.AddListener<Event_EntityDespawned>(OnEntityDespawned);
    }

    protected void OnEntityDespawned(Event_EntityDespawned evt) {
        RemoveEntityFromFactions(evt.entity);
    }

    public static Color GetColor(FactionId factionId) {
        switch(factionId) {
            case FactionId.Maas: return Color.red;
            case FactionId.XWA: return Color.green;
            default: return Color.white;
        }
    }

    private Dictionary<FactionId, Disposition> CreateDispositionMap(FactionId id) {
        Dictionary<FactionId, Disposition> map = new Dictionary<FactionId, Disposition>();
        foreach(FactionId factionId in Enum.GetValues(typeof(FactionId))) {
            if (factionId == id) continue;
            map[factionId] = factionDispositionMap[id.ToString() + ":" + factionId.ToString()];
        }
        return map;
    }

    public static void AddEntity(Entity entity) {
        for (int i = 0; i < factions.Count; i++) {
            factions[i].AddEntity(entity);
        }
    }

    public static Faction GetFaction(FactionId id) {
        return factions.Find((faction) => faction.id == id);
    }

    public static void SetFactionsHostile(FactionId factionId1, FactionId factionId2) {
        Faction f1 = factions.Find((f) => f.id == factionId1);
        Faction f2 = factions.Find((f) => f.id == factionId2);
        f1.SetFactionHostile(f2.id);
        f2.SetFactionHostile(f1.id);
    }

    public static void SetFactionsFriendly(FactionId factionId1, FactionId factionId2) {
        Faction f1 = factions.Find((f) => f.id == factionId1);
        Faction f2 = factions.Find((f) => f.id == factionId2);
        f1.SetFactionFriendly(f2.id);
        f2.SetFactionFriendly(f1.id);
    }

    public static void SetFactionsNeutral(FactionId factionId1, FactionId factionId2) {
        Faction f1 = factions.Find((f) => f.id == factionId1);
        Faction f2 = factions.Find((f) => f.id == factionId2);
        f1.SetFactionNeutral(f2.id);
        f2.SetFactionNeutral(f1.id);
    }

    public static void RemoveEntityFromFactions(Entity ent) {
        for (var i = 0; i < factions.Count; i++) {
            factions[i].RemoveEntity(ent);
        }
    }

    public static List<Entity> GetHostile(FactionId id) {
        return GetFaction(id).GetHostiles();
    }

    public static int GetHostileCount(FactionId id) {
        return GetFaction(id).HostileCount;
    }
}

