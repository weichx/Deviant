public class MissionEventCondition : Condition {

    private string eventName;
    private bool fired;

    public MissionEventCondition(string eventName) {
        this.eventName = eventName;
        this.fired = false;
    }

    public override bool Eval() {
        if (fired) return true;
        fired = MissionLog.EventFired(eventName);
        return fired;
    }
}

public partial class MissionScript {
    public MissionEventCondition MissionEventTriggered(string eventName) {
        return new MissionEventCondition(eventName);
    }
}