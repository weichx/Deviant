public class EntityDestroyedCondition : Condition {
    public string shipId;
    private bool destroyed;

    public EntityDestroyedCondition(string shipId) {
        this.destroyed = false;
        this.shipId = shipId;
    }

    public override bool Eval() {
        return destroyed || EntityManager.EntityDestroyed(shipId);
    }
}

public partial class MissionScript {
    public EntityDestroyedCondition EntityDestroyed(string entityId) {
        return new EntityDestroyedCondition(entityId);
    }
}