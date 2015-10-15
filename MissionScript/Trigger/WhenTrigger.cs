using System;
using UnityEngine;

public class WhenTrigger : Trigger {

    public WhenTrigger(Condition condition, float interval = -1) : base(condition, interval) { }

    public override TriggerStatus Run() {
        if (interval < 0 || timer.ReadyWithReset(interval)) {
            if (condition.Eval()) {
                if (action != null) action();
                status = TriggerStatus.Completed;
            } else {
                status= TriggerStatus.Pending;
            }
        }
        return status;
    }
}
