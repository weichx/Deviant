public class InvertedCondition : Condition {
    protected Condition condition;

    public InvertedCondition(Condition condition) {
        this.condition = condition;
    }

    public override bool Eval() {
        return !condition.Eval();
    }
}