using UnityEngine;
using System.Collections.Generic;

//[System.Serializable]
//public class WeaponData {
//    public string weaponId;
//    public float range;
//    public float fireRate;
//    public float aspectTime;
//    public float lifeTime;
//    public float accuracy;
//    public float hullDamage;
//    public float shieldDamage;
//    public float speed;

//    public float aspectFOV;
//    public float aspectRange;
//    public bool aspectSeeking;
//    //todo add type, ammo, charge time, impact layer, fire mode, linkable etc

//    public WeaponData(string weaponId) {
//        this.weaponId = weaponId;
//        WeaponDatabase.RegisterWeaponData(this.weaponId, this);
//        this.range = 500f;
//        this.fireRate = 1f;
//        this.aspectTime = -1;
//        this.lifeTime = -1;
//        this.accuracy = 1f; //replace with spread range
//        this.hullDamage = 1f;
//        this.shieldDamage = 1f;
//        this.speed = 100f;
//    }

//    public static WeaponData Clone(WeaponData data) {
//        return data.MemberwiseClone() as WeaponData;
//    }
//}

public class WeaponDatabase : MonoBehaviour {
    //consider having weapon database just be a list of weapon name strings
    //and rolling weapon data into the weapon instances themselves
    
    private static List<string> weaponList;
    private static string[] rawWeaponList;
    private static bool dirtyWeaponList;
    public static string DefaultWeapon = "Laser";

    static WeaponDatabase() {
        weaponList = new List<string>();
        //todo create weapon list for each entity class
        weaponList.Add("Laser");
        weaponList.Add("Vulcan");
        weaponList.Add("Beam");
        weaponList.Add("RaptorMissile");
        weaponList.Add("Flak");
        weaponList.Add("ParticleCannon");
        rawWeaponList = weaponList.ToArray();
    }

    public static string[] GetWeaponList() {
        if (dirtyWeaponList) {
            dirtyWeaponList = false;
            rawWeaponList = weaponList.ToArray();
        }
        return rawWeaponList;
    }

    public static int GetWeaponIndex(string weaponId) {
        return weaponList.IndexOf(weaponId);
    }
}