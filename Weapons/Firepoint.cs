using UnityEngine;

//todo since weapon groups can fire independent of others, 
//create an exclusion list and a link list for what can cannot fire together
//and to coordinate fire timing

//todo rename WeaponGroup to WeaponSet and allow sets to be grouped into WeaponGroups
public class Firepoint : MonoBehaviour {
    public string weaponGroupId = WeaponGroup.DefaultId;
}