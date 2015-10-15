using UnityEngine;

public class ComparisonCondition : Condition {

    protected ComparisonOperator op;
    protected NumericCondition condition1;
    protected NumericCondition condition2;

    public ComparisonCondition(NumericCondition c1, ComparisonOperator op, NumericCondition c2) {
        this.condition1 = c1;
        this.condition2 = c2;
        this.op = op;
    }

    public override bool Eval() {
        float c1Value = condition1.GetValue();
        float c2Value = condition2.GetValue();
        switch (op) {
            case ComparisonOperator.GreaterThan:
                return c1Value > c2Value;
            case ComparisonOperator.LessThan:
                return c1Value < c2Value;
            case ComparisonOperator.GreaterThanEqualTo:
                return c1Value >= c2Value;
            case ComparisonOperator.LessThanEqualTo:
                return c1Value <= c2Value;
            case ComparisonOperator.Equal:
                return c1Value == c2Value;
            case ComparisonOperator.NotEqual:
                return c1Value != c2Value;
        }
        return false;
    }
}