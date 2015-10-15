public class BooleanCondition : Condition {
    protected EqualityOperator op;
    protected Condition c1;
    protected Condition c2;

    public BooleanCondition(Condition c1, EqualityOperator op, Condition c2) {
        this.c1 = c1;
        this.c2 = c2;
        this.op = op;
    }

    public override bool Eval() {
        if (op == EqualityOperator.Equal) {
            return c1.Eval() == c2.Eval();
        } else {
            return c1.Eval() != c2.Eval();
        }
    }
}
