using UnityEngine;

[SelectionBase]
public class Entity : MonoBehaviour {

    protected EventManager eventManager;

    [Header("Meta Data")]

    public FactionId factionId;
    public EntitySize size = EntitySize.Small;
    public EntityType type = EntityType.Fighter;
    public float radius;

    [Header("Entity Info")]
    
    [Range(0f, 100f)]
    public float hullIntegrity = 100f;

    [Range(0f, 100f)]
    public float shieldIntegrity = 100f;

    public float speed;
    public string displayName;

    [HideInInspector]
    public WeaponSystem weaponSystem;

    [HideInInspector]
    public EngineSystem engineSystem;

    [HideInInspector]
    public Pilot pilot;


    public void Awake() {
        if (radius <= 0) radius = 10f; //todo do this better, use collider bounds
        weaponSystem = GetComponentInChildren<WeaponSystem>();
        engineSystem = GetComponentInChildren<EngineSystem>();
    }

    public void Start() {
        if(displayName == null || displayName == string.Empty) {
            int index = name.LastIndexOf('_');
            displayName = name.Substring(index + 1);
        }

        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        if(renderer) {
            var tracker = renderer.gameObject.AddComponent<VisiblitityTracker>();
            tracker.entity = this;
        }
        EntityManager.Add(this);
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
