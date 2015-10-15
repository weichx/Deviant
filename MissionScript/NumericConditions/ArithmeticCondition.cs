public class ArithmeticCondition : NumericCondition {
    protected NumericCondition c1;
    protected NumericCondition c2;
    protected ArithmeticOperator op;

    public ArithmeticCondition(NumericCondition c1, ArithmeticOperator op, NumericCondition c2) {
        this.c1 = c1;
        this.c2 = c2;
        this.op = op;
    }

    public override float GetValue() {
        switch (op) {
            case ArithmeticOperator.Plus:
                return c1.GetValue() + c2.GetValue();
            case ArithmeticOperator.Minus:
                return c1.GetValue() - c2.GetValue();
            case ArithmeticOperator.Multiply:
                return c1.GetValue() * c2.GetValue();
            case ArithmeticOperator.Modulus:
                return c1.GetValue() % c2.GetValue();
            case ArithmeticOperator.Divide:
                return c1.GetValue() / c2.GetValue();
        }
        return float.MinValue;
    }

    
}


/*
public TotalShipsDestroyedCondition : Condition {
    public override float GetValue() {
        return EntityDatabase.TotalShipsDestroyed;
    }
}

public float TotalShipsDestroyed {
    get { return new TotalShipsDestroyedCondition(); }
}

if(TotalShipsDestroyed - 2 > 5f & TimeSinceMissionStart % 2 == 1) {

}

When();

*/