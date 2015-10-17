using UnityEngine;

public class PlayerManager {

    private static Entity player;
    private static Transform playerViewpointTransform; 

    static PlayerManager() {
        player = GameObject.FindWithTag("Player").GetComponent<Entity>();
        playerViewpointTransform = player.transform.GetComponentInChildren<CameraEye>().transform;
    }

    public static Entity PlayerEntity {
        get { return player; }
    }

    public static PlayerPilot PlayerPilot {
        get { return player.pilot as PlayerPilot; }
    }

    public static Transform PlayerTransform {
        get { return player.transform; }
    }

    public static GameObject PlayerGameObject {
        get { return player.gameObject; }
    }

    public static Transform PlayerViewpointTransform {
        get { return playerViewpointTransform; }
    }

    public static EventManager PlayerEventManager {
        get { return player.EventManager; }
    }
}
