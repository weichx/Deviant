using UnityEngine;
using System.Collections.Generic;


public class FormationManager {
    //todo will need to spilt by faction / type / allowed eventually
    private static Formation formation; //temp

    static FormationManager() {
    }

    public static Formation CreateFormation() {
        formation = GameObject.Find("Formation").GetComponent<Formation>(); //temp
        return formation;
    }

    public static Formation GetAvailableFormation() {
        return formation;
    }
}

