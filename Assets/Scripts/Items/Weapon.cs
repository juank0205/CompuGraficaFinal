using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Weapon", menuName = "Items/Weapon")]
public class Weapon : Item {
    public int magazineSize;
    public int magazineCount;
    public float fireRate;
    public int damage;
    public GameObject prefab;
    public float range;
    public WeaponType type;
}

public enum WeaponType { Melee, Pistol }
