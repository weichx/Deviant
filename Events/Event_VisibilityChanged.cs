public class Event_EntityVisibilityChanged : GameEvent {
    public bool isVisible;
    public readonly Entity entity;

    public Event_EntityVisibilityChanged(Entity entity) {
        this.entity = entity;
        this.isVisible = false;
    }
}