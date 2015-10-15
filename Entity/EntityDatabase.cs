using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class EntityDatabaseEntry {
    public int enterCount;
    public int exitCount;
    public bool destroyed;
    public bool disabled;
    public int captureCount;
    public FactionId factionId;
    public EntitySize size;
    public EntityType type;
    public Entity entity;

    public EntityDatabaseEntry(Entity entity) {
        this.entity = entity;
        this.enterCount = 1;
        this.exitCount = 0;
        this.disabled = false;
        this.destroyed = false;
        this.factionId = entity.factionId;
        this.size = entity.size;
        this.type = entity.type;
    }

    public bool Active {
        get { return enterCount > 0 & exitCount < enterCount && !destroyed; }
    }
}

public static class EntityManager {
    public static readonly Dictionary<string, EntityDatabaseEntry> database;
    public static readonly List<Entity> entities;


    static EntityManager() {
        database = new Dictionary<string, EntityDatabaseEntry>();
        entities = new List<Entity>();
    }

    public static void Add(Entity entity) {
        var entry = new EntityDatabaseEntry(entity);
        Assert.IsNull(database.Get(entity.displayName), "EntityDatabase has already registered entity with the displayName: " + entity.displayName);
        database.Add(entity.displayName, entry);
        EventManager.Instance.QueueEvent(new Event_EntitySpawned(entity, TimeManager.Timestamp));
    }

    public static void Remove(Entity entity) {
        EventManager.Instance.QueueEvent(new Event_EntityDespawned(entity, TimeManager.Timestamp));
    }

    public static Entity GetEntity(string id) {
        EntityDatabaseEntry entry = null;
        database.TryGetValue(id, out entry);
        return entry.entity;
    }

    public static Entity[] GetEntities(string[] ids) {
        EntityDatabaseEntry entry;
        Entity[] ents = new Entity[ids.Length];
        for (int i = 0; i < ids.Length; i++) {
            if (database.TryGetValue(ids[i], out entry)) {
                ents[i] = entry.entity;
            } else {
                ents[i] = null;
            }
        }
        return ents;
    }

    public static bool EntitiesDestroyed(string[] entityIds) {
        EntityDatabaseEntry entry;
        for (int i = 0; i < entityIds.Length; i++) {
            if (database.TryGetValue(entityIds[i], out entry)) {
                if (!entry.destroyed) return false;
            } else {
                //todo throw exception if entity not in database?
            }
        }
        return true;
    }

    public static bool EntityDestroyed(string entityId) {
        EntityDatabaseEntry entry;
        if (database.TryGetValue(entityId, out entry)) {
            return entry.destroyed;
            //todo throw exception if entity not in database?
        }
        return false;
    }
}