using System;
using System.Collections.Generic;

public abstract class Trigger {
    protected Action action;
    protected Condition condition;
    protected TriggerStatus status;
    protected Timer timer;
    protected float interval;

    public Trigger(Condition condition, float interval = -1) {
        this.condition = condition;
        this.interval = interval;
        this.timer = new Timer(interval);
        this.status = TriggerStatus.Pending;
        TriggerRuntime.AddTrigger(this);
    }

    public abstract TriggerStatus Run();

    public Trigger Then(Action action) {
        this.action = action;
        return this;
    }

    public Trigger Do(Action action) {
        this.action = action;
        return this;
    }

    public void Reset() {
        status = TriggerStatus.Pending;
        timer.Reset(interval);
    }

    public void Pause() {
        status = TriggerStatus.Paused;
    }

    public void Resume() {
        status = TriggerStatus.Pending;
    }
}