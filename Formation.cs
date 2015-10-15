using UnityEngine;
using System.Collections.Generic;

public class Formation : MonoBehaviour {

    protected Pilot leader;
    protected List<Pilot> members;
    protected Dictionary<Pilot, FormationNode> nodeMap;
    protected FormationNode[] nodes;
    public float tension = 1f; //how tightly ships stay to their nodes. 1 tension = 1 ship radius

    public float LeaderSpeed {
        get { return leader.engines.speed; }
    }

    public Pilot Leader {
        get { return leader; }
    }

    void Awake() {
        nodeMap = new Dictionary<Pilot, FormationNode>();
        members = new List<Pilot>();
    }

    void Start() {
        nodes = GetComponentsInChildren<FormationNode>();
        System.Array.Sort(nodes);
        for (int i = 0; i < nodes.Length; i++) {
            nodes[i].formation = this;
        }
    }

    void Update() {
        if (leader != null) {
            transform.position = leader.transform.position;
            transform.rotation = leader.transform.rotation;
        }
    }

    public FormationNode GetNode(Pilot pilot) {
        FormationNode node;
        nodeMap.TryGetValue(pilot, out node);
        return node;
    }

    public void SetLeader(Pilot pilot) {
        leader = pilot;
        RemoveMember(leader);
        transform.position = leader.transform.position;
        leader.formation = this;
        leader.engines.MaxSpeed = FindSpeedCap();
    }

    public bool AddMember(Pilot pilot) {
        if(members.Count == nodes.Length) {
            return false;
        }
        if (pilot.formation) {
            pilot.formation.RemoveMember(pilot);
        }
        nodeMap.Add(pilot, nodes[members.Count]);
        members.Add(pilot);
        pilot.formation = this;
        leader.engines.MaxSpeed = FindSpeedCap();
        return true;
    }

    public void RemoveMember(Pilot pilot) {
        if(pilot.FormationLeader) {
            //todo do something here
        }
        pilot.formation = null;
        nodeMap.Remove(pilot);
        members.Remove(pilot);
    }

    public void Disband() {
        for (int i = 0; i < members.Count; i++) {
            Pilot pilot = members[i];
            pilot.formation = null;
            nodeMap.Remove(pilot);
            members.Remove(pilot);
        }
        members.Clear();
        nodeMap.Clear();
        leader.formation = null; 
    }

    //speed cap is 75% of slowest member's top speed
    protected float FindSpeedCap() {
        float minSpeed = leader.engines.MaxSpeed;
        for (var i = 0; i < members.Count; i++) {
            float speed = members[i].engines.MaxSpeed;
            if (minSpeed < speed) {
                minSpeed = speed;
            }
        }
        return minSpeed * 0.75f;
    }
}
