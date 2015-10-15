public class Event_EntitySpawned : GameEvent {

    public readonly Entity entity;
    public readonly float gameTime;

    public Event_EntitySpawned(Entity entity, float gameTime) {
        this.entity = entity;
        this.gameTime = gameTime;
    }

}