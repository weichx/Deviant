using System;

public class WhileTrigger : Trigger {
    protected int executedCount;
    protected int limit;

    public WhileTrigger(Condition condition, float interval = -1, int limit = -1) : base(condition, interval) {
        this.limit = limit;
        this.executedCount = 0;
    }

    public override TriggerStatus Run() {
        if ((limit > 0 && executedCount >= limit)) {
            status = TriggerStatus.Completed;
            return status;
        }
        if (interval < 0 || timer.ReadyWithReset(interval)) {
            if (condition.Eval()) {
                if(limit > 0) executedCount++;
                if (action != null) action();
            }
        }
        return TriggerStatus.Pending;
    }

    public WhileTrigger SetInterval(float interval) {
        this.interval = interval;
        this.timer.Reset(interval);
        return this;
    }

    public WhileTrigger Every(float interval) {
        this.interval = interval;
        this.timer.Reset(interval);
        return this;
    }

    public WhileTrigger Limit(int limit) {
        this.limit = limit;
        this.executedCount = 0;
        return this;
    }
}