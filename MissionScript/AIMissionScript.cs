using UnityEngine;
using System.Collections.Generic;

public class AIMissionScript : MissionScript {

    public Dictionary<string, AIGoal> goalMap;

    public void Start() {
        /*
        When(ShipsDestroyed("") & Integrity > 0.5f).Then(SetGoal(), SetGoalTenacity(4), );

        State("Phase2", Phase2);

        State("Phase3", Phase3)
            .OnEnter()
            .OnExit()
            .OnResume();
        
        AIGoal goal1 = DestroyShips(["", "", ""], Prioritizer);
        When(AtLeast_X_ShipsDestroyed(3, "", "").Then(SetState(""));
        
        public void State_Phase1OnInitialize() {

        }

        public void State_Phase1OnEnter() {
        
        }

        public void State_Phase1OnUpdate() {
        
        }

        public void State_Phase1OnExit() {
        
        }

        public void State_Phase1.OnResume() {
        
        }

        public void Phase1() {
            SetGoal(goal1);
            goal1.SetPrioritizer(fn);
            
            When(MissionPhase == "Phase2" & !goal1.Completed).Then(SetState("Phase2"));
            if(!ShipsDestroyed("").Eval()) {
                
            }
        }

        */
    }

    public void Update() {
        
    }
}

public class AIGoal {

}

public class DestroyShips : AIGoal {
    public List<string> shipList;

    public DestroyShips(string[] ships) {
        this.shipList = new List<string>(ships);
    }

    public DestroyShips(List<string> ships) {
        this.shipList = ships;
    }
}
/*
Enter Area
Reach Waypoint
Arrive At Location
Begin Wait
End Wait
Dock
*/