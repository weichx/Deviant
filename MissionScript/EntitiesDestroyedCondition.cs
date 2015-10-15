public class EntitiesDestroyedCondition : Condition {
    private string[] shipIds;

    public EntitiesDestroyedCondition(string[] shipIds) {
        this.shipIds = shipIds;
    }

    public override bool Eval() {
        return EntityManager.EntitiesDestroyed(shipIds);
    }
}

public partial class MissionScript {
    public EntitiesDestroyedCondition EntitiesDestroyed(string[] entityIds) {
        return new EntitiesDestroyedCondition(entityIds);
    }
}