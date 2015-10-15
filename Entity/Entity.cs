using UnityEngine;

[SelectionBase]
public class Entity : MonoBehaviour {
    [Header("Meta Data")]

    public FactionId factionId;
    public EntitySize size = EntitySize.Small;
    public EntityType type = EntityType.Fighter;
    public float radius;
    public string variation;

    [Header("Entity Info")]
    
    [Range(0f, 100f)]
    public float hullIntegrity = 100f;

    [Range(0f, 100f)]
    public float shieldIntegrity = 100f;

    public float speed;
    public float speedPotential;
    public float agilityRating;
    public Vector3 velocity;
    public float acceleration;
    public string displayName;

    [HideInInspector]
    public WeaponSystem weaponSystem;

    [HideInInspector]
    public EngineSystem engineSystem;

    [HideInInspector]
    public Pilot pilot;
    protected EventManager eventManager;

    public void Awake() {
        if (radius <= 0) radius = 10f; //todo do this better, use collider bounds
        weaponSystem = GetComponentInChildren<WeaponSystem>();
        engineSystem = GetComponentInChildren<EngineSystem>();
    }

    public void Start() {
        if(displayName == null || displayName == string.Empty) {
            displayName = name;
        }
        EntityManager.Add(this);
        FactionManager.AddEntity(this);
    }

    public void Update() {
        if (engineSystem) {
            speed = engineSystem.speed;
        }
    }

    public EventManager EventManager {
        get {
            if(eventManager == null) {
                eventManager = gameObject.AddComponent<EventManager>();
            }
            return eventManager;
        }
    }

    public bool Piloted {
        get { return pilot != null; }
    }

    public void ApplyDamage(Entity shooter, float damage) {
       // if (this == PlayerManager.PlayerEntity) return;
        hullIntegrity -= damage;
        var evt = new Event_EntityDamaged(this, shooter, damage);
        EventManager.Instance.QueueEvent(evt);
        if(eventManager != null) {
            eventManager.QueueEvent(evt);
        }
        if (this != PlayerManager.PlayerEntity && hullIntegrity <= 0f) {
            DeathManager.Instance.Destroy(this);
        }
    }
}
