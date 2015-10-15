public class Event_EntityDespawned : GameEvent {

    public readonly Entity entity;
    public readonly float gameTime;

    public Event_EntityDespawned(Entity entity, float gameTime) {
        this.entity = entity;
        this.gameTime = gameTime;
    }

}