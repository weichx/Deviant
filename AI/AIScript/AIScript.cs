using UnityEngine;

//public class AIScript : ScriptableObject {
//    public abstract void Update(Entity entity);
//
//   
//}
//
//public class Mission1AIGoal : AIScript {
//    public override void Update(Entity e) {
//        
//    }
//}

/*

 * void Objectives(Entity e) {
 *  DestroyShips("ship1", "ship2").Prioritize((s) => )
 * }
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
GoToVector(new Vector3(1, 1, 1));
GoToVector(new Vector3(3, 3, 3)).Arrive();

public abstract class EntityBaseScript {
    public static float Highest = 1f;

    public Entity entity;
    public AIPilot pilot;

    public EntityBaseScript(Entity entity) {
        this.entity = entity;
        this.pilot = entity.pilot;
    }

    public virtual void Goals() { }
    public virtual void Events() { }

    public void ApplyOrders() {
        if (this.pilot == null) return;

    }

    public void DestroyEntities(params string[] shipList) {
        AIGoalDescriptor desc = new AIGoalDescriptor();
    }

    public GoalDesc Goal(string description) {
        //        var desc = new GoalDesc(this.pilot, description);
        //        return desc;
        return null;
    }

    public class GoalDesc {
        protected float Highest = 1.0f;
        protected float Middle = 0.5f;
        protected float Lowest = 0.0001f;

        public string description;
        public AIPilot pilot;
        public GoalDesc(AIPilot pilot, string description) {
            this.pilot = pilot;
            this.description = description.TrimEnd();
        }

        public GoalDesc Priority(float priority) {
            return this;
        }

        public GoalDesc DestroyEntities(params string[] shipList) {
            //goal.aiState = AIStates.DestroyShips
            //goal.targetPrioritizer = delegate() {};
            return this;
        }

        public GoalDesc WarpOut(string warpPoint) {
            return this;
        }
    }
}

public class EntityScript : EntityBaseScript {

    public void Initialize() {
        //setup ignored things, example world state at init time and maybe do stuff
    }

    public override void Goals() {
                AIGoal goal = Goal("Enter Battle").ArriveFromWarp();
                Goal("Patrol Area").Waypoints("Waypoint Reference").CustomUpdate(() => );
               
                Repeat(WaypointGoal("Fly Waypoints", WaypointList)).While(delegate).Or().And()
                

                EscortGoal("HSC Bellatrix").Description("Escort HSC Bellatrix to jump point");

                ShipsInArea.GreaterThan(10),
                ShipsInArea().LessThan(11)
)
                


                Goal("Engage Hostiles In Range").DestroyEntitiesInRadius(500f)
                       .RepeatableSubgoal()
                       .SingleSubgoal()
                       .Subgoal()
                Goal("Arrive At Start Point").Arrive("Marker|Vector3|EntityId").DistanceThreshold(50f).Arrive(true).NonInterruptable();
                    Goal("Escort HSC Bellatrix").Escort("HSC Bellatrix")
                        .KeepDistance(500f)
                        .Prioritize(Prioritize.AntiCapitalSmallShips)
                        .WhenIdle("KeepFormation")

                Goal("KeepFormation").LeaderSelection(LeaderSelection.SquadronNumeric).Parallel("FlyWaypoints")
        
                Goal("Destroy Group Alpha").DestroyEntities("Alpha", "Beta")
                    .DeactivateOnEvent(DamageEvent("Entity*", Hull(-5), Shield(0.25f)));
                    .DeactivateOnEvent();
                
                Goal("Leave").WarpOut("WarpPoint1").ActivateOnMissionEvent("");
    }

    public override void Events() {
        When(TotalShipsInMission.GreaterThan(10), delegate())
    }

}

*/