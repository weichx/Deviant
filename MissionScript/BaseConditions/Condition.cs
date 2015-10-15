public abstract class Condition {

    public abstract bool Eval();

    public static Condition operator !(Condition c) {
        return new InvertedCondition(c);
    }

    public static OrCondition operator |(Condition c1, Condition c2) {
        return new OrCondition(c1, c2);
    }

    public static AndCondition operator &(Condition c1, Condition c2) {
        return new AndCondition(c1, c2);
    }

    public static Condition operator ==(Condition c1, Condition c2) {
        return new BooleanCondition(c1, EqualityOperator.Equal, c2);
    }

    public static Condition operator ==(Condition c1, bool b) {
        Condition c2 = Condition.True;
        if (!b) c2 = Condition.False;
        return new BooleanCondition(c1, EqualityOperator.Equal, c2);
    }

    public static Condition operator !=(Condition c1, Condition c2) {
        return new BooleanCondition(c1, EqualityOperator.NotEqual, c2);
    }

    public static Condition operator !=(Condition c1, bool b) {
        Condition c2 = Condition.True;
        if (!b) c2 = Condition.False;
        return new BooleanCondition(c1, EqualityOperator.NotEqual, c2);
    }

    public static TrueCondition True = new TrueCondition();
    public static FalseCondition False = new FalseCondition();

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}