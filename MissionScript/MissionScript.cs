using System;
using UnityEngine;
using System.Collections.Generic;

//when do these run?
//on events?
//periodically?
//every frame?

//major game events 
//EntityArrived
//EntityDeparted
//EntityDocked
//EntityDestroyed
//EntityDisabled
//EntityFactionChanged
//MissionPhaseChanged
//Custom Mission Events


//Update Times -- Run on configurable cycle
//             -- Custom Scheduler can be set per script
//             -- Defaults to 10 times per frame

//While --> once per frame?
//Every --> once per `arg` seconds (min 1 frame)
//Until --> once per frame
//If --> runs when first encountered, not again
//Else --> runs when first encountered, not again
//ElseIf --> runs when first encountered, not again
//When --> Every Major Mission Event, 10 times per second. Does not repeat
//WhenEver --> Every Major Mission Event, 10 times per second. Repeats 
//If(True).Do().ElseIf().Do().ElseIf().Do().Else(Action);


public class TimeElapsedCondition : NumericCondition {

    private Timer timer;

    public TimeElapsedCondition() {
        this.timer = new Timer();
    }

    public override float GetValue() {
        // Debug.Log("GetValue( )" + timer.ElapsedTime);
        return timer.ElapsedTime;
    }
}

//todo -- remove mono behavior and use a single scheduler
public partial class MissionScript : MonoBehaviour {
    public TrueCondition True = Condition.True;
    public FalseCondition False = Condition.False;


    //Trigger.Reset(); -- Everything starts over, If/Else/ElseIf will run again
    //Trigger.Pause(); -- Everything is paused
    //Trigger.Resume(); -- Everything resumes

    public void Triggers() {
        //Whenever(MissionPhaseIs("PhaseName"), () => { EnableGroup("A") });
        //Group("A", () => {});
        //Group("B", () => {});
        //DisableGroup("A");
        //EnableGroup("B");
        //ResetGroup("C");

        //WhenTrigger x = When(Something).Then(Something);
        //x.SetUpdateInterval(10f);

        //        When(True).Then(() => {
        //            When(True | !False).Then(SomeAction); //disable / enable?
        //            When(MissionEventTriggered("Alpha Warp In")).Then(SomeAction);
        //            //When(EntityDestroyed("Dauntless") & TimeSinceDestroyed("Dauntless") >= 10f)
        //            While(True).Do(SomeAction); //start / stopable?
        //            Until(True).Do(SomeAction); //start / stopable?
        ////            If(True).Do(SomeAction)
        ////            .Else(SomeAction)
        //            .ElseIf(True).Do(SomeAction)
        //   });
    }


    //Acts as a trigger, continuously observes conditions. Executes Then() once
    public WhenTrigger When(Condition condition) {
        return new WhenTrigger(condition);
    }

    public WhileTrigger While(Condition condition) {
        return new WhileTrigger(condition);
    }

    public TimeElapsedCondition TimeElapsed() {
        return new TimeElapsedCondition();
    }

//    public EveryTrigger Every(Condition condition, float interval) {
//        return new EveryTrigger(condition, interval);
//        if(condition) action(); 
//    }

    private TriggerContainer triggerContainer = new TriggerContainer();
    public TriggerContainer Every(float interval) {
        this.triggerContainer.interval = interval;
        return triggerContainer;
    }
}

public class TriggerContainer {
    public float interval;
    public int limit;
//    public WhenTrigger When(Condition condition) {
//        return new WhenTrigger(condition, interval);
//    }
//
//    public WhileTrigger While(Condition condition) {
//        return new WhileTrigger(condition, interval);
//    }

    public WhileTrigger If(Condition condition) {
        WhileTrigger trigger = new WhileTrigger(condition, interval, limit);
        this.limit = -1;
        return trigger;
    }

    public WhileTrigger Unless(Condition condition) {
        WhileTrigger trigger = new WhileTrigger(!condition, interval, limit);
        this.limit = -1;
        return trigger;
    }

    public TriggerContainer Limit(int limit) {
        this.limit = limit;
        return this;
    }
}