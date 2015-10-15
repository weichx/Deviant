public class AndCondition : Condition {
    protected Condition c1;
    protected Condition c2;

    public AndCondition(Condition c1, Condition c2) {
        this.c1 = c1;
        this.c2 = c2;
    }

    public override bool Eval() {
     return c1.Eval() && c2.Eval();
    }

}
