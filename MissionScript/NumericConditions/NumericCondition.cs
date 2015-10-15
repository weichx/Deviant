public abstract class NumericCondition : Condition {
    public abstract float GetValue();

    public override bool Eval() {
        return true;
    }

    public static ArithmeticCondition operator +(NumericCondition c1, float v) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Plus, new FloatConstantCondition(v));
    }

    public static ArithmeticCondition operator +(NumericCondition c1, ArithmeticCondition c2) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Plus, c2);
    }

    public static ArithmeticCondition operator -(NumericCondition c1, ArithmeticCondition c2) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Minus, c2);
    }

    public static ArithmeticCondition operator -(NumericCondition c1, float v) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Minus, new FloatConstantCondition(v));
    }

    public static ArithmeticCondition operator *(NumericCondition c1, ArithmeticCondition c2) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Multiply, c2);
    }

    public static ArithmeticCondition operator *(NumericCondition c1, float v) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Multiply, new FloatConstantCondition(v));
    }

    public static ArithmeticCondition operator /(NumericCondition c1, ArithmeticCondition c2) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Divide, c2);
    }

    public static ArithmeticCondition operator /(NumericCondition c1, float v) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Divide, new FloatConstantCondition(v));
    }

    public static ArithmeticCondition operator %(NumericCondition c1, float v) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Modulus, new FloatConstantCondition(v));
    }

    public static ArithmeticCondition operator %(NumericCondition c1, ArithmeticCondition c2) {
        return new ArithmeticCondition(c1, ArithmeticOperator.Modulus, c2);
    }

    public static ComparisonCondition operator >(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.GreaterThan, c2);
    }

    public static ComparisonCondition operator >(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.GreaterThan, new FloatConstantCondition(v));
    }

    public static ComparisonCondition operator >=(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.GreaterThanEqualTo, c2);
    }

    public static ComparisonCondition operator >=(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.GreaterThanEqualTo, new FloatConstantCondition(v));
    }

    public static ComparisonCondition operator <(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.LessThan, c2);
    }

    public static ComparisonCondition operator <(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.LessThan, new FloatConstantCondition(v));
    }

    public static ComparisonCondition operator <=(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.LessThanEqualTo, c2);
    }

    public static ComparisonCondition operator <=(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.LessThanEqualTo, new FloatConstantCondition(v));
    }

    public static ComparisonCondition operator ==(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.Equal, c2);
    }

    public static ComparisonCondition operator ==(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.Equal, new FloatConstantCondition(v));
    }

    public static ComparisonCondition operator !=(NumericCondition c1, ArithmeticCondition c2) {
        return new ComparisonCondition(c1, ComparisonOperator.NotEqual, c2);
    }

    public static ComparisonCondition operator !=(NumericCondition c1, float v) {
        return new ComparisonCondition(c1, ComparisonOperator.NotEqual, new FloatConstantCondition(v));
    }

    public override bool Equals(object obj) {
        return base.Equals(obj);
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}