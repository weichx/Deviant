public class AIEngineSystem : EngineSystem {

    public override void FixedUpdate() {
        if (flightControls == null) return;
        AdjustThrottleByVelocity();
    }
}