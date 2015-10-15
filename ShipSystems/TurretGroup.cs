using UnityEngine;

//Note WeaponGroup is basically identical but not using a shared implementation
//because each turret would need its own weaponSystem and I don't want to manage
//weapon systems on a per turret basis. Still need to figure out how AI interacts
//with turrets.

public class TurretGroup : MonoBehaviour { 
    public string groupId;
    public string weaponId = WeaponDatabase.DefaultWeapon;
    public const string DefaultId = "default";
}