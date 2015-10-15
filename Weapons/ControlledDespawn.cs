
public class ControlledDespawn : AbstractWeaponSpawnable {
    public bool shouldDespawn = false;

    public void Update() {
        if (shouldDespawn) {
            shouldDespawn = false;
            spawner.Despawn(this);
        }
    }
}