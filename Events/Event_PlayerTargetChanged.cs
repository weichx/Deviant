public class Event_PlayerTargetChanged : GameEvent {

    public readonly Entity newTarget;
    public readonly Entity oldTarget;

    public Event_PlayerTargetChanged(Entity newTarget, Entity oldTarget) {
        this.newTarget = newTarget;
        this.oldTarget = oldTarget;
    }

}

