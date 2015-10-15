public class FloatConstantCondition : NumericCondition {
    protected float value;

    public FloatConstantCondition(float value) {
        this.value = value;
    }

    public override float GetValue() {
        return value;
    }
}