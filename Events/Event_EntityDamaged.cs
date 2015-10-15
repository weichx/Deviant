using UnityEngine;

public class Event_EntityDamaged : GameEvent {
    public readonly Entity victim;
    public readonly Entity shooter;
    public readonly float damage;

    public Event_EntityDamaged(Entity victim, Entity shooter, float damage) {
        this.victim = victim;
        this.shooter = shooter;
        this.damage = damage;
    }
}